using System;
using System.Windows.Forms;
using System.Timers;
using AutoUpdaterDotNET;
using System.Text;
using System.Runtime.Remoting.Messaging;
using CoreOSC;

namespace VRChatify
{
    public partial class MainWindow : Form
    {
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
    }
}