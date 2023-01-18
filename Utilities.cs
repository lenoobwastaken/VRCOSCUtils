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
//using SDL2;
namespace VRCOSCUtils
{
    internal class Utilities
    {
        private static  MediaManager mediaManager ;
        private static MediaSession currentSession = null;

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
        public static void InitMedia()
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
            SendSpotify();
            Console.ReadLine();
        }
        public static bool NoStats = false;
        public static void SendSpotify()
        {
            while (true)
            {
                if (NoStats == false)
                {
                    Program.oscSender.Send(new OscMessage("/chatbox/input", $"{MainInfo()} || CPU: {GetCPU()} || RAM: {GetRAM()} || GPU: {GetGPU()}", true, true));
                    LogUtils.Log("Sent!");
                }
                else
                {
                    Program.oscSender.Send(new OscMessage("/chatbox/input", $"{MainInfo()}", true, true));
                    LogUtils.Log("Sent!");

                    Thread.Sleep(1500);
                }
                //Thread.Sleep(1500);
            }
        }
        public static void SendSpotifyNoStats()
        {
            while (true)
            {
                LogUtils.Log($"{MainInfo()}");
                Thread.Sleep(1500);
            }
        }

        public static string MainInfo()
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
