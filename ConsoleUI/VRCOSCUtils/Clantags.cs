using CoreOSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VRCOSCUtils
{
    internal static class Clantag
    {
        public static List<string> joe;
        public static int FrameCount;
        public static void SendFrames()
        {
            while (true)
            {
                for (int i = 0; i < joe.Count; i++)
                {
                    LogUtils.Log(joe[i]);
                    Program.oscSender.Send(new OscMessage("/chatbox/input", joe[i], true, true));
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
        public static Task Aimware()
        {
            string[] Aimware = new string[] { "---AIMWARE.NET---", "----AIMWARE.NET--", "-----AIMWARE.NET-", "----AIMWARE.NET", "T------AIMWARE.NE", "ET------AIMWARE.N", "NET------AIMWARE.", ".NET------AIMWARE", "E.NET------AIMWAR", "RE.NET------AIMWA", "ARE.NET------AIMW", "WARE.NET------AIM", "MWARE.NET------AI", "IMWARE.NET------A", "AIMWARE.NET------", "-AIMWARE.NET-----", "--AIMWARE.NET----", "---AIMWARE.NET---" };
            while (true)
            {
                foreach (var strin in Aimware)
                {
                    LogUtils.Log(strin);
                    Program.oscSender.Send(new OscMessage("/chatbox/input", strin, true, true));
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

                    Program.oscSender.Send(new OscMessage("/chatbox/input", Neverlose[i], true, true));
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

                    Program.oscSender.Send(new OscMessage("/chatbox/input", gamesense[i], true, true));
                    Thread.Sleep(1500);
                }
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

                        Program.oscSender.Send(new OscMessage("/chatbox/input", CurrentString, true, true));

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
                        Program.oscSender.Send(new OscMessage("/chatbox/input", s, true, true));
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
    }
}
