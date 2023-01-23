using CoreOSC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRCOSCUtils
{
    internal static class SpacebarHook
    {
        #region https://github.com/NYAN-x-CAT/LimeLogger/blob/master/LimeLogger/LimeLogger.cs 
        public static Stopwatch Watch;

        public static int SpacebarPS = 0;
        public static string GetElapsed()
        {
            TimeSpan WatchTimeSpan = Watch.Elapsed;

            return String.Format("{0:00}:{1:00}:{2:00}",
            WatchTimeSpan.Hours, WatchTimeSpan.Minutes, WatchTimeSpan.Seconds);
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                return SetWindowsHookEx(WHKEYBOARDLL, proc, GetModuleHandle(curProcess.ProcessName), 0);
            }
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
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
            Program.oscSender.Send(new OscMessage("/chatbox/input", $"Spacebar Counter: {NumSpace} || Spacebar Per Second: {SpacebarPS} || Elapsed: {GetElapsed()}", true, true));

            SpacebarPS = 0;



        }
        public static int NumSpace = 0;
        public static string KeyboardLayout(uint vkCode)
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

        public static string GetActiveWindowTitle()
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


        public const int WM_KEYDOWN = 0x0100;
        public static LowLevelKeyboardProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        public static int WHKEYBOARDLL = 13;

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

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
    }

}
