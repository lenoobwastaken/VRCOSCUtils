using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsMediaController.MediaManager;
using WindowsMediaController;
using System.Diagnostics;
using System.Threading;
using System.Management;
using Windows.Media.Control;
using Windows.Media.Core;
using CoreOSC;
using Windows.ApplicationModel.Store;
using Microsoft.Diagnostics.Tracing.Session;
//using SDL2;
namespace VRCOSCUtils
{
    internal class MediaUtilities
    {
        public static string CurrentSongCheck = null;

        public static string CurrentSong = null;

        public static string CurrentSound = null;


        private static  MediaManager mediaManager ;
        private static MediaSession currentSession = null;
        public static string GetSoundPad()
        {
            var SoundPadProc = Process.GetProcessesByName("Soundpad").FirstOrDefault(SP => !string.IsNullOrEmpty(SP.MainWindowTitle));
            if (SoundPadProc == null)
            {
                LogUtils.Error("[Error] Soundpad is not opened");
            }
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            double available = 0, capacity = 0;
            foreach (ManagementObject mo1 in moc1)
            {
                capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
            }
            moc1.Dispose();
            cimobject1.Dispose();


            ManagementClass cimobject2 = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObjectCollection moc2 = cimobject2.GetInstances();
            foreach (ManagementObject mo2 in moc2)
            {
                available += ((Math.Round(Int64.Parse(mo2.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1)));

            }
            moc2.Dispose();
            cimobject2.Dispose();
            CurrentSound = SoundPadProc.MainWindowTitle;
            if (CurrentSound == "Soundpad")
            {
                return $"Idling on SoundPad || CPU: {Math.Round(MediaUtilities.getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: {Math.Round(MediaUtilities.GetGPUUsage())}%";
            }
            return $"{SoundPadProc.MainWindowTitle} || CPU: {Math.Round(MediaUtilities.getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: {Math.Round(MediaUtilities.GetGPUUsage())}%";
        }
        public static float GetGPUUsage()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var counterNames = category.GetInstanceNames();
                var gpuCounters = new List<PerformanceCounter>();
                var result = 0f;

                foreach (string counterName in counterNames)
                {
                    if (counterName.EndsWith("engtype_3D"))
                    {
                        foreach (PerformanceCounter counter in category.GetCounters(counterName))
                        {
                            if (counter.CounterName == "Utilization Percentage")
                            {
                                gpuCounters.Add(counter);
                            }
                        }
                    }
                }

                gpuCounters.ForEach(x =>
                {
                    _ = x.NextValue();
                });

                Thread.Sleep(1000);

                gpuCounters.ForEach(x =>
                {
                    result += x.NextValue();
                });

                return result;
            }
            catch
            {
                LogUtils.Error("[Error] Grabbing gpu usage");
                return 0f;
            }
        }
        public static string GetGPU()
        {
            return $"{Math.Round(GetGPUUsage())}%";
        }
        public static string GetRAM()
        {
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            double available = 0, capacity = 0;
            foreach (ManagementObject mo1 in moc1)
            {
                capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
            }
            moc1.Dispose();
            cimobject1.Dispose();


            ManagementClass cimobject2 = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObjectCollection moc2 = cimobject2.GetInstances();
            foreach (ManagementObject mo2 in moc2)
            {
                available += ((Math.Round(Int64.Parse(mo2.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1)));

            }
            moc2.Dispose();
            cimobject2.Dispose();
            return $"{(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%";
        }
        public static float getCurrentCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var s = cpuCounter.NextValue();
            Thread.Sleep(1000);
            return (dynamic)cpuCounter.NextValue();
        }
        public static string GetFPS()
        {
         //  var s =  SDL2.SDL.SDL_GetPerformanceCounter();
           return "";
        }
        public static string GetCPU()
        {
            return $"{Math.Round(getCurrentCpuUsage())}%";
        }
        public  enum MediaType : int
        {
            Spotify = 0,
            Soundpad = 1,
        }
        public static void InitMedia(MediaType type)
        {
            LogUtils.Log("Would you like to show your pc stats?\ny or n");

            var s = Console.ReadLine();
            if (s.Contains("n"))
            {
                NoStats = true;
            }
            mediaManager = new MediaManager();

            mediaManager.OnAnySessionOpened += MediaManager_OnAnySessionOpened;
            mediaManager.OnAnySessionClosed += MediaManager_OnAnySessionClosed;
            mediaManager.OnAnyPlaybackStateChanged += MediaManager_OnAnyPlaybackStateChanged;
            mediaManager.OnAnyMediaPropertyChanged += MediaManager_OnAnyMediaPropertyChanged;

            mediaManager.Start();

#if !DEBUG

       
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

            // Thread thOutput = new Thread(OutputThreadProc);
            //  thOutput.IsBackground = true;
            //  thOutput.Start();

            switch (type)
            {
                case MediaType.Spotify:
                    SendSpotify();

                    break;
                case MediaType.Soundpad:
                    SendSoundpad();
                    break;
                default:
                    Console.WriteLine("This should not happen");
                    break;
            }
            Console.ReadLine();
            m_EtwSession.Dispose();

#endif
#if DEBUG
        
#endif
        }
        public const int EventID_D3D9PresentStart = 1;
        public const int EventID_DxgiPresentStart = 42;

        //ETW provider codes
        public static readonly Guid DXGI_provider = Guid.Parse("{CA11C036-0102-4A2D-A6AD-F03CFED5D3C9}");
        public static readonly Guid D3D9_provider = Guid.Parse("{783ACA0A-790E-4D7F-8451-AA850511C6B9}");

        static TraceEventSession m_EtwSession;
        static Dictionary<int, TimestampCollection> frames = new Dictionary<int, TimestampCollection>();
        static Stopwatch watch = null;
        static object sync = new object();

        static void EtwThreadProc()
        {
            //start tracing
            m_EtwSession.Source.Process();
        }
        public static string currentfps = null;
        public static bool NoStats = false;
        public static void SendSpotify()
        {
            while (true)
            {
                if (NoStats == true)

                {
                    long t1, t2;
                    long dt = 2000;
                    //  Console.Clear();
                    //   Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString());
                    // Console.WriteLine();

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
                                currentfps = $"{Math.Round((double)count / dt * 1000.0)}";
                                Program.oscSender.Send(new OscMessage("/chatbox/input", $"{SpotifyInfo()} || FPS: {currentfps}", true, true));
                                LogUtils.Log("Sent!");
                                //  Console.WriteLine(currentfps);
                            }


                        }
                    }
                  //  Program.oscSender.Send(new OscMessage("/chatbox/input", $"{SpotifyInfo()} || FPS: {FPSUtils.currentfps}", true, true));
                 //   LogUtils.Log("Sent!");

                    Thread.Sleep(1600);
                }
                else
                {
                    long t1, t2;
                    long dt = 2000;
                    //  Console.Clear();
                    //   Console.WriteLine(DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString());
                    // Console.WriteLine();

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
                                currentfps = $"{Math.Round((double)count / dt * 1000.0)}";
                                //  Console.WriteLine(currentfps);
                                Program.oscSender.Send(new OscMessage("/chatbox/input", $"{SpotifyInfo()} || CPU: {GetCPU()} || RAM: {GetRAM()} || GPU: {GetGPU()} || FPS: {currentfps}", true, true));
                                LogUtils.Log("Sent!");

                            }


                        }
                    }
                }
                //Thread.Sleep(1500);
            }
        }

        public static void SendSoundpad()
        {
            while (true)
            {
                if (NoStats == true)

                {
                    Program.oscSender.Send(new OscMessage("/chatbox/input", $"{SpotifyInfo()}", true, true));
                    LogUtils.Log("Sent!");

                    Thread.Sleep(1500);
                }
                else
                {
                    Program.oscSender.Send(new OscMessage("/chatbox/input", $"{SpotifyInfo()} || CPU: {GetCPU()} || RAM: {GetRAM()} || GPU: {GetGPU()}", true, true));
                    LogUtils.Log("Sent!");
                }
                //Thread.Sleep(1500);
            }
        }

        public static string SpotifyInfo()
        {
            if (currentSession  != null && currentproperties != null )
            {
                if (currentSession.ControlSession.GetPlaybackInfo().PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
                {
                    return $"[Paused] {currentproperties.Artist} - {currentproperties.Title} || {currentSession.ControlSession.GetTimelineProperties().Position.ToString(@"mm\:ss")}/{currentSession.ControlSession.GetTimelineProperties().EndTime.ToString(@"mm\:ss")}";

                }
                return $"{currentproperties.Artist} - {currentproperties.Title} || {currentSession.ControlSession.GetTimelineProperties().Position.ToString(@"mm\:ss")}/{currentSession.ControlSession.GetTimelineProperties().EndTime.ToString(@"mm\:ss")}";
            }
            
            return "";
        }

        public static string SoundpadInfo()
        {
            var SoundPadProc = Process.GetProcessesByName("Soundpad").FirstOrDefault(SP => !string.IsNullOrEmpty(SP.MainWindowTitle));
            if (SoundPadProc != null && NoStats == false)
            {
                if (SoundPadProc.MainWindowTitle == "Soundpad")
                {
                    return $"Nothing Playing || CPU: {GetCPU()} || RAM: {GetRAM()} || GPU: {GetGPU()}";
                }
                var s = SoundPadProc.MainWindowTitle.Split('-');
                
                return $"Playing - {s[1]} || CPU: {GetCPU()} || RAM: {GetRAM()} || GPU: {GetGPU()}";
            }
            if (SoundPadProc != null && NoStats == true)
            {
                if (SoundPadProc.MainWindowTitle == "Soundpad")
                {
                    return $"Nothing Playing";
                }
                var s = SoundPadProc.MainWindowTitle.Split('-');

                return $"Playing - {s[1]}";
            }
            return "";
        }

#region Credits to dubya dude for this
        private static void MediaManager_OnAnySessionOpened(MediaManager.MediaSession session)
        {
            currentSession = session;
            
        }
        private static void MediaManager_OnAnySessionClosed(MediaManager.MediaSession session)
        {
        }
        private static void MediaManager_OnAnyPlaybackStateChanged(MediaManager.MediaSession sender, GlobalSystemMediaTransportControlsSessionPlaybackInfo args)
        {
          //  LogUtils.Log($"{sender.ControlSession.GetPlaybackInfo().PlaybackRate}");
        }
        private static void MediaManager_OnAnyMediaPropertyChanged(MediaManager.MediaSession sender, GlobalSystemMediaTransportControlsSessionMediaProperties args)
        {
            if (sender.Id == "Spotify.exe")
            {
                currentSession = sender;
                currentproperties = args;

            }
            
            //LogUtils.Log(MainInfo());
        }
        public static GlobalSystemMediaTransportControlsSessionMediaProperties currentproperties = null;

#endregion
    }
}
