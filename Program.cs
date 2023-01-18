using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreOSC;
using VRCOSCUtils;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;
using Timer = System.Threading.Timer;
using WindowsMediaController;
namespace VRCOSCUtils
{
    class Program
    {
        private static readonly MediaManager mediaManager = new MediaManager();

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
        public static UDPDuplex oscListener;


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
                return $"Idling on SoundPad || CPU: {Math.Round(getCurrentCpuUsage())}% || RAM: {(Math.Round((capacity - available) / capacity * 100, 0)).ToString()}%  || GPU: { Math.Round(GetGPUUsage())}%";
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

        public static string CurrentSongCheck = null;

        public static string CurrentSong = null;

        public static string CurrentSound = null;

        

        public static string GetElapsed()
        {
            TimeSpan WatchTimeSpan = Watch.Elapsed;
            
            return String.Format("{0:00}:{1:00}:{2:00}",
            WatchTimeSpan.Hours, WatchTimeSpan.Minutes, WatchTimeSpan.Seconds); 
        }
        
        #region https://github.com/NYAN-x-CAT/LimeLogger/blob/master/LimeLogger/LimeLogger.cs 


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                return SetWindowsHookEx(WHKEYBOARDLL, proc, GetModuleHandle(curProcess.ProcessName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool capsLock = (GetKeyState(0x14) & 0xffff) != 0;
                bool shiftPress = (GetKeyState(0xA0) & 0x8000) != 0 || (GetKeyState(0xA1) & 0x8000) != 0;
                string currentKey = KeyboardLayout((uint)vkCode);

              

                if (((Keys)vkCode).ToString() == "Space" && t.Enabled == false)
                {
                    NumSpace += 1;
                    SpacebarPS += 1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        public static System.Timers.Timer t = new System.Timers.Timer();

        public static void Joe()
        {

        }
        public static void Send(Object source)
        {
            LogUtils.Log($"Spacebar Counter: {NumSpace} || Spacebar Per Second: {SpacebarPS} || Elapsed: {GetElapsed()}");
            oscSender.Send(new OscMessage("/chatbox/input", $"Spacebar Counter: {NumSpace} || Spacebar Per Second: {SpacebarPS} || Elapsed: {GetElapsed()}" , true, true));

            SpacebarPS = 0;



        }
        public static int NumSpace = 0;
        private static string KeyboardLayout(uint vkCode)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] vkBuffer = new byte[256];
                if (!GetKeyboardState(vkBuffer)) return "";
                uint scanCode = MapVirtualKey(vkCode, 0);
                IntPtr keyboardLayout = GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), out uint processId));
                ToUnicodeEx(vkCode, scanCode, vkBuffer, sb, 5, 0, keyboardLayout);
                return sb.ToString();
            }
            catch { }
            return ((Keys)vkCode).ToString();
        }

        private static string GetActiveWindowTitle()
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                GetWindowThreadProcessId(hwnd, out uint pid);
                Process p = Process.GetProcessById((int)pid);
                string title = p.MainWindowTitle;
                if (string.IsNullOrWhiteSpace(title))
                    title = p.ProcessName;
                return title;
            }
            catch (Exception)
            {
                return "???";
            }
        }


        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        private static int WHKEYBOARDLL = 13;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);
        #endregion
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
        public static int SpacebarPS = 0;
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

                        Thread.Sleep(1500);
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
                        Thread.Sleep(1500);

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
        public static Task Aimware()
        {
            string[] Aimware = new string[] { "---AIMWARE.NET---", "----AIMWARE.NET--", "-----AIMWARE.NET-", "----AIMWARE.NET", "T------AIMWARE.NE", "ET------AIMWARE.N", "NET------AIMWARE.", ".NET------AIMWARE", "E.NET------AIMWAR", "RE.NET------AIMWA", "ARE.NET------AIMW", "WARE.NET------AIM", "MWARE.NET------AI", "IMWARE.NET------A", "AIMWARE.NET------", "-AIMWARE.NET-----", "--AIMWARE.NET----", "---AIMWARE.NET---" };
            while (true)
            {
                foreach (var strin in Aimware)
                {
                    LogUtils.Log(strin);
                    oscSender.Send(new OscMessage("/chatbox/input", strin, true, true));
                    Thread.Sleep(1500);
                }
            }
        }
        public static Task NeverLose()
        {
            string[] Neverlose = new string[] { " | ", @" |\\ ", @" |\\| ", @" N ", @" N3 ", @" Ne ", @" Ne\\ ", @" Ne\\/ ", @" Nev ", @" Nev3 ", @" Neve ", @" Neve| ", @" Neve|2 ", @" Neve|_ ", @" Nevel ", @" Nevel0 ", @" Nevelo ", @" Nevelo5 ", @" Nevelos ", @" Nevelos3 ", @" Nevelose " };
            while (true)
            {
                for (int i = 0; i < Neverlose.Length; i++)
                {
                    LogUtils.Log(Neverlose[i]);

                    oscSender.Send(new OscMessage("/chatbox/input", Neverlose[i], true, true));
                    Thread.Sleep(1500);


                }



            }
        }
        public static Task Gamesense()
        {
            string[] gamesense = new string[] { "g", "ga", "gam", "game", "games", "gamese", "gamesen", "gamesens", "gamesense" };
            while (true)
            {
                for (int i = 0; i < gamesense.Length; i++)
                {
                    LogUtils.Log(gamesense[i]);

                    oscSender.Send(new OscMessage("/chatbox/input", gamesense[i], true, true));
                    Thread.Sleep(1500);
                }
            }
        }
        
        public static void Misc()
        {
            LogUtils.Clear();
            LogUtils.Error("Please enter valid input\n");

            LogUtils.Log("Options:\n1. Aimware Clantag || 2. NeverLose Clantag || 3. gamesense Clantag");
            var s = Console.ReadLine();
            switch (s)
            {

                case "1":
                    Aimware();
                    break;
                case "2":
                    NeverLose();
                    break;
                case "3":
                    Gamesense();
                    break;
                default:

                    Misc();
                    break;
            }
        }
        public static void OnReceive(OscPacket bytes)
        {
           // Console.WriteLine($"{((OscMessage)bytes).Address} {string.Join(" ", ((OscMessage)bytes).Arguments)}" );
           if (((OscMessage)bytes).Address.ToString() == "/avatar/parameters/Upright")
           {
                if (string.Join(" ", ((OscMessage)bytes).Arguments).Contains("1"))
                {
                    Console.WriteLine("Crouching");
                }
                else
                {
                    Console.WriteLine("Standing");

                }
            }
        }
        public static void Redo()
        {

            LogUtils.Clear();
            LogUtils.Error("Please enter valid input\n");

            LogUtils.Log("Options:\n1. Spotify and PC Stats || 2. Animated ClanTag (Must Edit CustomName.txt) || 3. Display Soundpad and PC Stats \n|| 4. Misc || 5. SpaceBar Counter || 6. KeyFramed Text");
            var s = Console.ReadLine();
            switch (s)
            {
                
                case "1":
                    Utilities.InitMedia();
                    break;
                case "2":
                    ClanTagChanger(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt"));
                    break;
                case "3":
                    UpdateOSCSP();
                    break;
                case "4":
                    Misc();
                    break;
                case "5":
                    _hookID = SetHook(_proc);
                    Joe();
                    break;
                case "6":
                    KeyFrameInput();
                    break;  
                default:

                    Redo();
                    break;
            }
        }
        public static List <string> joe;
        public static int FrameCount;
        public static void SendFrames()
        {
            while (true)
            {
                for (int i = 0; i < joe.Count; i++)
                {
                    LogUtils.Log(joe[i]);
                    oscSender.Send(new OscMessage("/chatbox/input", joe[i], true, true));
                    Thread.Sleep(1500);
                }
            }
        }
        public static void KeyFrameInput()
        {
            joe = new List<string>();
            if (!string.IsNullOrEmpty(File.ReadAllText($"{Environment.CurrentDirectory}\\KeyFrameConfig.txt")))
            {
                LogUtils.Log("Do you want to load your previous frames config?\ny for yes || n for no");
                var userinput4 = Console.ReadLine();
                if (userinput4 == "y")
                {
                    var filestring = File.ReadAllText($"{Environment.CurrentDirectory}\\KeyFrameConfig.txt").Split(',');
                    foreach (var file in filestring)
                    {
                        joe.Add(file);
                    }
                    SendFrames();

                }
                else
                {
                    LogUtils.Log("Enter frame count:");
                    var userinput = Console.ReadLine();
                    FrameCount = int.Parse(userinput);

                    for (int i = 0; i < FrameCount; i++)
                    {
                        LogUtils.Log($"Enter frame {i} value:");
                        var userinput2 = Console.ReadLine();
                        joe.Add(userinput2);
                    }
                    LogUtils.Log("Would you like to save your frames to config?\ny for yes || n for no");
                    var userinput3 = Console.ReadLine();

                    if (userinput3 == "y")
                    {
                        var filelist = new StringBuilder();
                        for (int i = 0; i < joe.Count; i++)
                        {
                            filelist.Append(joe[i] + ",");
                        }
                        File.WriteAllText($"{Environment.CurrentDirectory}\\KeyFrameConfig.txt", filelist.ToString());
                    }

                    SendFrames();
                }
            }
            else
            {
                LogUtils.Log("Enter frame count:");
                var userinput = Console.ReadLine();
                FrameCount = int.Parse(userinput);

                for (int i = 0; i < FrameCount; i++)
                {
                    LogUtils.Log($"Enter frame {i} value:");
                    var userinput2 = Console.ReadLine();
                    joe.Add(userinput2);
                }
                LogUtils.Log("Would you like to save your frames to config?\ny for yes || n for no");
                var userinput3 = Console.ReadLine();

                if (userinput3 == "y")
                {
                    var filelist = new StringBuilder();
                    for (int i = 0; i < joe.Count; i++)
                    {
                        filelist.Append(joe[i] + ",");
                    }
                    File.WriteAllText($"{Environment.CurrentDirectory}\\KeyFrameConfig.txt", filelist.ToString());
                }

                SendFrames();
            }
          
        
        }
        public static Stopwatch Watch;
        static void Main(string[] args)
        {
            //oscListener = new UDPDuplex("127.0.0.1", 9000, 9001, OnReceive );
            // Console.ReadKey();
#if !DEBUG

            oscSender = new UDPSender("127.0.0.1", 9000);
            Init();
            LogUtils.Logo();
            LogUtils.Log("Options:\n1. Spotify and PC Stats || 2. Animated ClanTag (Must Edit CustomName.txt) || 3. Display Soundpad and PC Stats \n|| 4. Misc || 5. SpaceBar Counter || 6. KeyFramed Text");
            var s = Console.ReadLine();
            switch (s)
            {
                case "1":
                    Utilities.InitMedia();
                    break;
                case "2":
                    ClanTagChanger(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt"));
                    break;
                case "3":
                    UpdateOSCSP();
                    break;
                case "4":
                    Misc();
                    break;
                case "5":
                    _hookID = SetHook(_proc);
                    Timer t = new Timer(Send, null, 0, 1500);

                    Watch = Stopwatch.StartNew();
                    Watch.Start();
                    Application.Run();

                    break;
                case "6":
                    KeyFrameInput();
                    break;
                default:
                    Redo();
                    break;
            }   
#endif

        }

    }
}

