using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SharpOSC;
using VRCOSCUtils;
using System.IO;

namespace VRCOSCUtils
{
    class Program
    {

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
        public static UDPSender oscSender;

        public static string GetSpotifySong()
        {
            //https://stackoverflow.com/questions/37854194/get-current-song-name-for-a-local-application
            var SpotifyProcess = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));
            if (SpotifyProcess == null)
            {
                LogUtils.Error("[Error] Spotify is not opened");
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
            CurrentSong = SpotifyProcess.MainWindowTitle;
            if (CurrentSong == "Spotify Free" || CurrentSong == "Spotify Premium")
            {
                return $"Idling on Spotify || CPU: {Math.Round(getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: { Math.Round(GetGPUUsage())}%";
            }
            if (CurrentSong == "Spotify" || CurrentSong == "Advertisement")
            {
                return $"Listening To A Ad :( || CPU: {Math.Round(getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: { Math.Round(GetGPUUsage())}%";

            }
            return $"{SpotifyProcess.MainWindowTitle} || CPU: {Math.Round(getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: { Math.Round(GetGPUUsage())}%";
        }
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
                return "Idling on SoundPad";
            }
            return $"{SoundPadProc.MainWindowTitle} || CPU: {Math.Round(getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: { Math.Round(GetGPUUsage())}%";
        }
        public static float getCurrentCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var s = cpuCounter.NextValue();
            Thread.Sleep(1000);
            return (dynamic)cpuCounter.NextValue();
        }
        public static float s;
        public static string CurrentSongCheck = null;

        public static string CurrentSong = null;

        public static string CurrentSound = null;

        public static Task UpdateOSC()
        {
            while (true)
            {
                oscSender.Send(new OscMessage("/chatbox/input", GetSpotifySong(), true, true));
                LogUtils.Log("Sent Current Song!");
                Thread.Sleep(10);
                CurrentSongCheck = CurrentSong;

            }
        }
        public static Task UpdateOSCSP()
        {
            while (true)
            {
                oscSender.Send(new OscMessage("/chatbox/input", GetSoundPad(), true, true));
                LogUtils.Log("Sent Current Song!");
                Thread.Sleep(10);
                CurrentSongCheck = CurrentSong;

            }
        }
        public static Task ClanTagChanger(string CustomText)
        {
            CustomText += " ";
            string CurrentString = "";
            int currentindex = 0;
            while (true)
            {
                foreach (var cha in CustomText)
                {
                    try
                    {
                        currentindex += 1;
                        CurrentString += cha;
                        Console.WriteLine(CurrentString);

                        oscSender.Send(new OscMessage("/chatbox/input", CurrentString, true, true));

                        Thread.Sleep(2000);
                        if (cha == ' ')
                        {
                            currentindex -= 1;

                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                foreach (char cha in CurrentString)
                {
                    try
                    {
                        var s = CurrentString.Remove(currentindex);
                        Console.WriteLine(s);
                        oscSender.Send(new OscMessage("/chatbox/input", s, true, true));
                        currentindex -= 1;
                        Thread.Sleep(2000);

                    }
                    catch (Exception e)
                    {

                    }
                }
                CurrentString = null;
            }
        }
        public static void Init()
        {
            if (!File.Exists(Environment.CurrentDirectory + "\\CustomName.txt"))
            {
                File.Create(Environment.CurrentDirectory + "\\CustomName.txt");
                File.WriteAllText(Environment.CurrentDirectory + "\\CustomName.txt", "VRCOSCSPOTIFY");
            }
            if (File.Exists(Environment.CurrentDirectory + "\\CustomName.txt") && string.IsNullOrEmpty(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt")))
            {
                File.WriteAllText(Environment.CurrentDirectory + "\\CustomName.txt", "VRCOSCSPOTIFY");
            }
        }
        public static void Redo()
        {

            LogUtils.Clear();
            LogUtils.Error("Please enter valid input\n");

            LogUtils.Log("Options:\n1. Spotify and PC Stats || 2. Animated ClanTag (Must Edit CustomName.txt) || 3. Display Soundpad and PC Stats");
            var s = Console.ReadLine();
            switch (s)
            {
                case "1":
                    UpdateOSC();
                    break;
                case "2":
                    ClanTagChanger(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt"));
                    break;
                case "3":
                    UpdateOSCSP();
                    break;
                default:

                    Redo();
                    break;
            }
        }
        static void Main(string[] args)
        {
            oscSender = new UDPSender("127.0.0.1", 9000);
            Init();
            LogUtils.Logo();
            LogUtils.Log("Options:\n1. Spotify and PC Stats || 2. Animated ClanTag (Must Edit CustomName.txt) || 3. Display Soundpad and PC Stats");
            var s = Console.ReadLine();
            switch (s)
            {
                case "1":
                    UpdateOSC();
                    break;
                case "2":
                    ClanTagChanger(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt"));
                    break;
                case "3":
                    UpdateOSCSP();
                    break;

                default:
                    Redo();
                    break;
            }
        }
    }
}
