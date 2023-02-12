using System;
using System.Collections.Generic;
using CoreOSC;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Microsoft.Diagnostics.Tracing.Session;
using Windows.ApplicationModel.Store;
using System.Runtime.CompilerServices;
namespace VRChatify
{
    public class TimestampCollection
    {
        const int MAXNUM = 1000;

        public string Name { get; set; }

        List<long> timestamps = new List<long>(MAXNUM + 1);
        object sync = new object();

        //add value to the collection
        public void Add(long timestamp)
        {
            lock (sync)
            {
                timestamps.Add(timestamp);
                if (timestamps.Count > MAXNUM) timestamps.RemoveAt(0);
            }
        }

        //get the number of timestamps withing interval
        public int QueryCount(long from, long to)
        {
            int c = 0;

            lock (sync)
            {
                foreach (var ts in timestamps)
                {
                    if (ts >= from && ts <= to) c++;
                }
            }
            return c;
        }
    }


    public static class VRChatify
    {
        public static bool debugging = false;
        public static string Version = "1.0.5";
        public static UDPSender oscSender;
        public static UDPListener oscReceiver;
        private static MainWindow mainWindow;
        public static VMediaManager mediaManager;

        public static string CurrentSongCheck = null;

        public static string CurrentSong = null;

        public static string CurrentSound = null;

        public static string ClanTag = "";
        public static List<string> ClanTagStrings;
        public static int ClanTagIndex = 0;
        public const int EventID_D3D9PresentStart = 1;
        public const int EventID_DxgiPresentStart = 42;

        //ETW provider codes
        public static readonly Guid DXGI_provider = Guid.Parse("{CA11C036-0102-4A2D-A6AD-F03CFED5D3C9}");
        public static readonly Guid D3D9_provider = Guid.Parse("{783ACA0A-790E-4D7F-8451-AA850511C6B9}");

        static TraceEventSession m_EtwSession;
        public static Dictionary<int, TimestampCollection> frames = new Dictionary<int, TimestampCollection>();
       public static Stopwatch watch = null;
        static object sync = new object();

        public static void SendChatMessage(string message)
        {
            oscSender.Send(new OscMessage("/chatbox/input", message, true, true));
            VRChatifyUtils.DebugLog("Updated ChatBox!");

            CurrentSongCheck = CurrentSong;
        }
        static void EtwThreadProc()
        {
            //start tracing
            m_EtwSession.Source.Process();
        }

        static void Main()
        {
            oscSender = new UDPSender("127.0.0.1", 9000);

            //var s = "\0";
          //  VRChatify.SendChatMessage("");
            //Console.ReadLine();
#if !DEBUG
            oscSender = new UDPSender("127.0.0.1", 9000);

            oscReceiver = new UDPListener(9001, OnOscPacket);
            mediaManager = new VMediaManager();
            mediaManager.Init();
            VRChatifyUtils.Logo();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            m_EtwSession = new TraceEventSession("mysess");
            m_EtwSession.StopOnDispose = true;
            m_EtwSession.EnableProvider("Microsoft-Windows-D3D9");
            m_EtwSession.EnableProvider("Microsoft-Windows-DXGI");

            //handle event
            m_EtwSession.Source.AllEvents += data =>
            {
                //filter out frame presentation events
                if (((int)data.ID == EventID_D3D9PresentStart && data.ProviderGuid == D3D9_provider) ||
                ((int)data.ID == EventID_DxgiPresentStart && data.ProviderGuid == DXGI_provider))
                {
                    int pid = data.ProcessID;
                    long t;

                    lock (sync)
                    {
                        t = watch.ElapsedMilliseconds;

                        //if process is not yet in Dictionary, add it
                        if (!frames.ContainsKey(pid))
                        {
                            frames[pid] = new TimestampCollection();

                            string name = "";
                            var proc = Process.GetProcessById(pid);
                            if (proc != null)
                            {
                                using (proc)
                                {
                                    name = proc.ProcessName;
                                }
                            }
                            else name = pid.ToString();

                            frames[pid].Name = name;
                        }

                        //store frame timestamp in collection
                        frames[pid].Add(t);
                    }
                }
            };

            watch = new Stopwatch();
            watch.Start();

            Thread thETW = new Thread(EtwThreadProc);
            thETW.IsBackground = true;
            thETW.Start();
            Thread thETW1 = new Thread(ThreadFPS);
            thETW1.IsBackground = true;
            thETW1.Start();

            mainWindow = new MainWindow();
            Application.Run(mainWindow);
#endif

        }
        public static string currentfps = null;
        private  static void joes(string j)
        {
            currentfps = j;
        }
        public static void ThreadFPS()
        {
            long t1, t2;
            long dt = 2000;

            lock (sync)
            {
                t2 = watch.ElapsedMilliseconds;
                t1 = t2 - dt;

                foreach (var x in frames.Values)
                {
                    if (x.Name == "VRChat")
                    {
                        //Console.Write("VRChat: ");
                        //get the number of frames
                        int count = x.QueryCount(t1, t2);

                        //calculate FPS
                        //  Console.WriteLine("{0} FPS", (double)count / dt * 1000.0);
                        joes( $"{Math.Round((double)count / dt * 1000.0)}");
                      //  Program.oscSender.Send(new OscMessage("/chatbox/input", $"{SpotifyInfo()} || FPS: {currentfps}", true, true));
                       // LogUtils.Log("Sent!");
                        //  Console.WriteLine(currentfps);
                    }


                }
            }

        }
        public static List<Button> GenerateSessionButtons()
        {
            List<Button> buttons = new List<Button>();
            int BaseLocX = 3;
            int BaseLocY = 15;
            foreach (var session in mediaManager.GetMediaManager().CurrentMediaSessions)
            {
                VRChatifyUtils.DebugLog("Generating Button");
                Button button = new Button
                {
                    ForeColor = System.Drawing.SystemColors.ControlText,
                    Location = new System.Drawing.Point(BaseLocX, BaseLocY),
                    Name = session.Key,
                    Size = new System.Drawing.Size(178, 23),
                    TabIndex = 2,
                    Text = session.Value.Id,
                    UseVisualStyleBackColor = true
                };
                button.Click += new EventHandler(DynamicButton_Click);
                BaseLocY += 25;
                buttons.Add(button);
            }
            return buttons;
        }

        private static void DynamicButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            mediaManager.setCurrentSession(mediaManager.GetMediaManager().CurrentMediaSessions[btn.Name]);
            VRChatifyUtils.DebugLog($"Current session set to: {btn.Name}");
        }

        private static void OnOscPacket(OscPacket packet)
        {
            if (!(packet is OscMessage message)
                || message.Address == null
                || message.Arguments == null
                || message.Arguments.Count == 0)
            {
                return;
            }
            var value = message.Arguments[0];
            if (value == null || !(value is bool boolean) || !boolean)
            {
                return;
            }

            Task.Run(async () => {
                var address = message.Address;
                if (address == "/avatar/parameters/back")
                {
                    VRChatifyUtils.DebugLog("Back");
                    await mediaManager.GetCurrentSession()?.ControlSession.TrySkipPreviousAsync();
                }
                else if (address == "/avatar/parameters/skip")
                {
                    VRChatifyUtils.DebugLog("Skip");
                    await mediaManager.GetCurrentSession()?.ControlSession.TrySkipNextAsync();
                }
                else if (address == "/avatar/parameters/paused")
                {
                    VRChatifyUtils.DebugLog("paused");
                    await mediaManager.GetCurrentSession()?.ControlSession.TryTogglePlayPauseAsync();
                }
                return Task.CompletedTask;
            });
        }

        public static MainWindow GetMainWindow()
        {
            return mainWindow;
        }
        

    }
}