using DiscordRPC;
using DiscordRPC.Logging;
namespace VRChatify
{
    class PresenceManager
    {
        private static RichPresence presence = new RichPresence()
        {
            Details = "Using VRchatify",
            State = "Created by AkiraDev#5057 && lenoob#2972",
            Timestamps = Timestamps.Now,
            Assets = new Assets()
            {
                LargeImageKey = "https://github.com/lenoobwastaken/BRUCE-CLIENT/blob/main/dsafsadfasdfwaefwqf.gif?raw=true",
                LargeImageText = "Vrchatify by bwmp and lenoob",
                SmallImageKey = "https://files.akiradev.xyz/VRchatify/VRChatify.png"
            },
            Buttons = new Button[]
                {
                    new Button()
                    {
                        Label = "Github",
                        Url = "https://github.com/Oli-idk/VRChatify"
                    }
                }
        };

        private static DiscordRpcClient client;

        public static void InitRPC()
        {

            client = new DiscordRpcClient("1044239130881687652")
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };

            client.OnReady += (sender, e) =>
            {
                VRChatifyUtils.Log($"Received Ready from user {e.User.Username}");
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                VRChatifyUtils.Log($"Received Update! {e.Presence}");
            };

            client.Initialize();

            client.SetPresence(presence);
        }
        public static void KillRPC()
        {
            client.Dispose();
        }
        public static void UpdateDetails(string details)
        {
            presence.Details = details;
            client.SetPresence(presence);
        }
    }
}
