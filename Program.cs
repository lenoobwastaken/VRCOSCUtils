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

      
        public static UDPSender oscSender;
        public static UDPDuplex oscListener;

        
      

        public static int SpacebarPS = 0;
       
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
       
        
        public static void Misc()
        {
            LogUtils.Clear();
            LogUtils.Error("Please enter valid input\n");

            LogUtils.Log("Options:\n1. Aimware Clantag || 2. NeverLose Clantag || 3. gamesense Clantag");
            var s = Console.ReadLine();
            switch (s)
            {

                case "1":
                    Clantag.Aimware();
                    break;
                case "2":
                    Clantag.NeverLose();
                    break;
                case "3":
                    Clantag.Gamesense();
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

            LogUtils.Log("Options:\n1. Spotify || 2. Animated ClanTag (Must Edit CustomName.txt) || 3. Soundpad \n|| 4. Misc || 5. SpaceBar Counter || 6. KeyFramed Text");
            var s = Console.ReadLine();
            switch (s)
            {
                
                case "1":
                    MediaUtilities.InitMedia(MediaUtilities.MediaType.Spotify);
                    break;
                case "2":
                    Clantag.ClanTagChanger(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt"));
                    break;
                case "3":
                    MediaUtilities.InitMedia(MediaUtilities.MediaType.Soundpad);
                    break;
                case "4":
                    Misc();
                    break;
                case "5":
                    SpacebarHook._hookID = SpacebarHook.SetHook(SpacebarHook._proc);
                    SpacebarHook.Joe();
                    break;
                case "6":
                    Clantag.KeyFrameInput();
                    break;  
                default:

                    Redo();
                    break;
            }
        }

        static void Main(string[] args)
        {
            //oscListener = new UDPDuplex("127.0.0.1", 9000, 9001, OnReceive );
            // Console.ReadKey();
#if !DEBUG

            oscSender = new UDPSender("127.0.0.1", 9000);
            Init();
            LogUtils.Logo();
            LogUtils.Log("Options:\n1. Spotify || 2. Animated ClanTag (Must Edit CustomName.txt) || 3. Soundpad \n|| 4. Misc || 5. SpaceBar Counter || 6. KeyFramed Text");
            var s = Console.ReadLine();
            switch (s)
            {
                case "1":
                    MediaUtilities.InitMedia(MediaUtilities.MediaType.Spotify);
                    break;
                case "2":
                    Clantag.ClanTagChanger(File.ReadAllText(Environment.CurrentDirectory + "\\CustomName.txt"));
                    break;
                case "3":
                    MediaUtilities.InitMedia(MediaUtilities.MediaType.Soundpad);
                    break;
                case "4":
                    Misc();
                    break;
                case "5":
                    SpacebarHook._hookID = SpacebarHook.SetHook(SpacebarHook._proc);
                    Timer t = new Timer(SpacebarHook.Send, null, 0, 1500);

                   SpacebarHook.Watch = Stopwatch.StartNew();
                    SpacebarHook.Watch.Start();
                    Application.Run();

                    break;
                case "6":
                    Clantag.KeyFrameInput();
                    break;
                default:
                    Redo();
                    break;
            }   
#endif
#if DEBUG
          
#endif 
        }

    }
}

