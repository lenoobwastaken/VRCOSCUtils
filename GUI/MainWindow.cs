using System;
using System.Windows.Forms;
using System.Timers;
using AutoUpdaterDotNET;
using System.Text;
using System.Runtime.Remoting.Messaging;
using CoreOSC;
using System.IO;
using System.Runtime.InteropServices;

namespace VRChatify
{
    public partial class MainWindow : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

        [DllImport("psapi.dll")]
        private static extern uint GetModuleBaseNameA(IntPtr hWnd, IntPtr hModule, StringBuilder lpBaseName, uint nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint PROCESS_QUERY_INFORMATION = 0x0400;
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint MAX_PATH = 260;

        public static string GetFocused()
        {
            // Get the handle of the foreground window
            IntPtr foregroundWindow = GetForegroundWindow();

            // Get the process ID of the foreground window
            GetWindowThreadProcessId(foregroundWindow, out uint processId);

            // Get a handle to the process
            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);

            // Get the process name
            StringBuilder processName = new StringBuilder((int)MAX_PATH);
            if (GetModuleBaseNameA(processHandle, IntPtr.Zero, processName, MAX_PATH) == 0)
            {
                // If it's null, assume it's VRChat
                CloseHandle(processHandle);
                return "VRChat.exe";
            }

            // Print the process name

            // Close the process handle
            CloseHandle(processHandle);

            return processName.ToString();
        }

        private static System.Timers.Timer OSCTimer;
        private static string oscText = Config.GetConfig("oscText") ?? "{SONG} - {ARTIST} | CPU: {CPU}% | RAM: {RAM}% | GPU: {GPU}% | Time: {TIME} | {CLANTAG}";
        public MainWindow()
        {
            //AutoUpdater.InstalledVersion = new Version(VRChatify.Version);
            //AutoUpdater.Start("https://raw.githubusercontent.com/Oli-idk/VRChatify/main/AutoUpdateInfo.xml");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //set window title to app version
            Text = $"VRCOSCUtils";
            //   oscMessage.Text = oscText;
            ClanTag.Text = Config.GetConfig("clantag") ?? "VRChatify";
            VRChatify.ClanTagStrings = VRChatifyUtils.ClanTagText(ClanTag.Text);
            UpdateSessionList();
        }

        private void PresenceToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (presenceToggle.Checked)
            {
                PresenceManager.InitRPC();
            }
            else
            {
                PresenceManager.KillRPC();
            }
        }
        public static StringBuilder oscMsg = new StringBuilder();
        public static void AppendToSB(string par)
        {
            if (!oscMsg.ToString().Contains(par))
            {
                oscMsg.Append(par);

            }
        }
        public static void joemethod()
        {

        }
        private void OSCToggle_CheckedChanged(object sender, EventArgs e)
        {
            //if checkbox checked start 5 second repeating timer else stop it
            if (OSCToggle.Checked)
            {
                foreach (var x in VRChatify.frames.Values)
                {
                    if (x.Name == "VRChat")
                    {
                        vrcproc = x;
                    }
                }
                // oscMsg = new StringBuilder();
                (Spotify == true ? new Action(() => AppendToSB($"{VRChatify.mediaManager.GetSongArtist()} - {VRChatify.mediaManager.GetSongName()} || {VMediaManager.currentSession.ControlSession.GetTimelineProperties().Position.ToString(@"mm\:ss")}/{VMediaManager.currentSession.ControlSession.GetTimelineProperties().EndTime.ToString(@"mm\:ss")}")) : new Action(joemethod))();
                (FPS == true ? new Action(() => AppendToSB($" || FPS: {VRChatify.currentfps}")) : new Action(joemethod))();
                (Tabbed == true ? new Action(() => AppendToSB($" || Focused In: {GetFocused()}")) : new Action(joemethod))();

                (Stats == true ? new Action(() => AppendToSB($" || CPU: {Math.Round(VRChatifyUtils.GetCpuUsage()).ToString()}% || RAM: {VRChatifyUtils.GetRamUsage()}% || GPU: {VRChatifyUtils.GetGPUUsage()}")) : new Action(joemethod))();

                (Clant == true ? new Action(() => AppendToSB($" || ClanTag: {VRChatify.ClanTagStrings[VRChatify.ClanTagIndex]}")) : new Action(joemethod))();
                (LocalTime == true ? new Action(() => AppendToSB($" || My Time: {DateTime.Now.ToString("h:mm:ss tt")}")) : new Action(joemethod))();

                // .Replace("{SONG}", VRChatify.mediaManager.GetSongName())
                // .Replace("{ARTIST}", VRChatify.mediaManager.GetSongArtist())
                // .Replace("{SPOTIFY}", VRChatifyUtils.GetSpotifySong())
                // .Replace("{CPU}", Math.Round(VRChatifyUtils.GetCpuUsage()).ToString())
                // .Replace("{GPU}", Math.Round(VRChatifyUtils.GetGPUUsage()).ToString())
                // //.Replace("{RAM-AVAILABLE}", VRChatifyUtils.GetRamAvailable().ToString())
                //// .Replace("{RAM-CAPACITY}", VRChatifyUtils.GetRamUsage().ToString())
                //// .Replace("{RAM-USED}", VRChatifyUtils.GetRamUsed().ToString())
                // .Replace("{RAM}", VRChatifyUtils.GetRamUsage().ToString())
                // .Replace("{TIME}", DateTime.Now.ToString("h:mm:ss tt"))
                // .Replace("{MTIME}", DateTime.Now.ToString("HH:mm"))
                // .Replace("{WINDOW}", VRChatifyUtils.GetActiveWindowTitle())
                // .Replace("{DURATION}", VRChatify.mediaManager.GetSongDuration().ToString(@"mm\:ss"))
                // .Replace("{POSITION}", VRChatify.mediaManager.GetCurrentSongTime().ToString(@"mm\:ss"))
                // .Replace("{CLANTAG}", VRChatify.ClanTagStrings[VRChatify.ClanTagIndex]);

                VRChatify.SendChatMessage(oscMsg.ToString());
                oscMsg.Clear();

                SetTimer();
            }
            else
            {
                OSCTimer.Stop();
                OSCTimer.Dispose();
                oscMsg.Clear();
            }
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            OSCTimer = new System.Timers.Timer(2500);
            // Hook up the Elapsed event for the timer. 
            OSCTimer.Elapsed += OnTimedEvent;
            OSCTimer.AutoReset = true;
            OSCTimer.Enabled = true;
        }
        static object sync = new object();
        public static bool IsCustom = false;
        public static TimestampCollection vrcproc;
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            long t1, t2;
            long dt = 2000;
            lock (sync)
            {
                
                t2 = VRChatify.watch.ElapsedMilliseconds;
                t1 = t2 - dt;

                int count = vrcproc.QueryCount(t1, t2);
                if (IsCustom == false)
                {

                
                    (Spotify == true ? new Action(() => AppendToSB($"{VRChatify.mediaManager.GetSongArtist()} - {VRChatify.mediaManager.GetSongName()} || {VMediaManager.currentSession.ControlSession.GetTimelineProperties().Position.ToString(@"mm\:ss")}/{VMediaManager.currentSession.ControlSession.GetTimelineProperties().EndTime.ToString(@"mm\:ss")}")) : new Action(joemethod))();
                    (FPS == true ? new Action(() => AppendToSB($" || FPS: {Math.Round((double)count / dt * 1000.0)}")) : new Action(joemethod))();

                    (Stats == true ? new Action(() => AppendToSB($" || CPU: {Math.Round(VRChatifyUtils.GetCpuUsage()).ToString()}% || RAM: {VRChatifyUtils.GetRamUsage()}% || GPU: {VRChatifyUtils.GetGPUUsage()}")) : new Action(joemethod))();

                    (Clant == true ? new Action(() => AppendToSB($" || ClanTag: {VRChatify.ClanTagStrings[VRChatify.ClanTagIndex]}")) : new Action(joemethod))();
                    (LocalTime == true ? new Action(() => AppendToSB($" || My Time: {DateTime.Now.ToString("h:mm:ss tt")}")) : new Action(joemethod))();


                    VRChatify.SendChatMessage(oscMsg.ToString());
                    oscMsg.Clear();

                    if (VRChatify.ClanTagIndex >= VRChatify.ClanTagStrings.Count - 1)
                    {
                        VRChatify.ClanTagIndex = 0;
                    }
                    else
                    {
                        VRChatify.ClanTagIndex += 1;
                    }
                }
                else if (IsCustom == true)
                {
                    VRChatify.Custom();
                    if (VRChatify.ClanTagIndex >= VRChatify.ClanTagStrings.Count - 1)
                    {
                        VRChatify.ClanTagIndex = 0;
                    }
                    else
                    {
                        VRChatify.ClanTagIndex += 1;
                    }
                }
            }

        }

        private void OSCMessage_TextChanged(object sender, EventArgs e)
        {
            Config.SetConfig("oscText", oscMessage.Text);
            oscText = oscMessage.Text;
        }

        private void PresenceUpdateButton_Click(object sender, EventArgs e)
        {
            PresenceManager.UpdateDetails(presenceDetails.Text);
        }

        private void ForceUpdateSessions_Click(object sender, EventArgs e) => UpdateSessionList();

        public void UpdateSessionList()
        {
            SessionHolder.Invoke(new MethodInvoker(delegate { SessionHolder.Controls.Clear(); }));
            foreach (var item in VRChatify.GenerateSessionButtons())
            {
                VRChatifyUtils.DebugLog($"Adding Buton {item.Text}");
                SessionHolder.Invoke(new MethodInvoker(delegate { SessionHolder.Controls.Add(item); }));
                VRChatifyUtils.DebugLog($"Added Buton {item.Text}");
            }
        }
        public static bool Spotify = false;
        public static bool Stats = false;
        public static bool Clant = false;
        public static bool FPS = false;
        public static bool LocalTime = false;
        public static bool Tabbed = false;
        private void DebugLogging_CheckedChanged(object sender, EventArgs e)
        {
            VRChatify.debugging = DebugLogging.Checked;
        }

        private void ClanTag_TextChanged(object sender, EventArgs e)
        {
            Config.SetConfig("clantag", ClanTag.Text);
            if (ClanTag.Text == "")
            {
                ClanTag.Text = " ";
            }
            VRChatify.ClanTagStrings = VRChatifyUtils.ClanTagText(ClanTag.Text);
            VRChatify.ClanTagIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //LOCAL TIME TOGGLE
            LocalTime = checkBox5.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //SPOTIFY TOGGLE
            Spotify = checkBox1.Checked;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //STATS TOGGLE
            Stats = checkBox2.Checked;

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //FPS TOGGLE
            FPS = checkBox3.Checked;

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //CLANTAG TOGGLE
            Clant = checkBox4.Checked;

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            IsCustom = checkBox6.Checked;
        }
        public static string test = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = $"{Environment.CurrentDirectory}",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                test = File.ReadAllText(openFileDialog1.FileName);
                if (!string.IsNullOrEmpty(test))
                {
                    richTextBox1.Text = test;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = string.Format("{0}Text files (*.txt)|*.txt|All files (*.*)|*.*", "ARG0"),
                RestoreDirectory = true,
                ShowHelp = true,
                CheckFileExists = false
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);



        }

        private void listView1_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Tabbed = checkBox7.Checked;

        }
    }
}