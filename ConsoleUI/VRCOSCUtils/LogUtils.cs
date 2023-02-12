using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colorful;
using Console = Colorful.Console;
 
namespace VRCOSCUtils
{ 
    public static class LogUtils 
    {
        public static void Error(string Message)
        {
            Console.Write($"{Message}\n", System.Drawing.Color.Red);
        }
        public static void Log(string Message)
        {
            Console.Write("[");
            Console.Write($"{DateTime.Now}", System.Drawing.Color.Green);
            Console.Write("] ");

            Console.Write("[");
            Console.Write("VRCOSC", System.Drawing.Color.Green);
            Console.Write("] ");
            Console.Write(Message);
            Console.WriteLine();
        }
        public static void Logo()
        {
            Console.WriteLine("█░█ █▀█ █▀▀ █▀█ █▀ █▀▀ █▀ █▀█ █▀█ ▀█▀ █ █▀▀ █▄█", System.Drawing.Color.Green);
            Console.WriteLine("▀▄▀ █▀▄ █▄▄ █▄█ ▄█ █▄▄ ▄█ █▀▀ █▄█ ░█░ █ █▀░ ░█░", System.Drawing.Color.Green);
        }
        public static void Clear()
        {
            Console.Clear();
            Console.WriteLine("█░█ █▀█ █▀▀ █▀█ █▀ █▀▀ █▀ █▀█ █▀█ ▀█▀ █ █▀▀ █▄█", System.Drawing.Color.Green);
            Console.WriteLine("▀▄▀ █▀▄ █▄▄ █▄█ ▄█ █▄▄ ▄█ █▀▀ █▄█ ░█░ █ █▀░ ░█░", System.Drawing.Color.Green);

        }
    }
}
