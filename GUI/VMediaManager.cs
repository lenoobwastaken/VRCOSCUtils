using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WindowsMediaController;

namespace VRChatify
{
    public class VMediaManager  
    {

        private static readonly MediaManager mediaManager = new MediaManager();
        public static MediaManager.MediaSession currentSession = null;
      

        public void Init()
        {
            mediaManager.Start();
            mediaManager.OnAnySessionOpened += MediaManager_OnAnySessionOpened;
            mediaManager.OnAnySessionClosed += MediaManager_OnAnySessionClosed;
        }

        private void MediaManager_OnAnySessionOpened(MediaManager.MediaSession mediaSession)
        {
            VRChatifyUtils.DebugLog($"Session Opened: {mediaSession.Id}");
            VRChatify.GetMainWindow().UpdateSessionList();
        }

        private void MediaManager_OnAnySessionClosed(MediaManager.MediaSession mediaSession)
        {
            VRChatifyUtils.DebugLog($"Session Closed: {mediaSession.Id}");
            VRChatify.GetMainWindow().UpdateSessionList();
        }

        public MediaManager.MediaSession GetCurrentSession()
        {
            if (currentSession == null)
            {
                try
                {
                    return currentSession = mediaManager.CurrentMediaSessions.First().Value;
                }
                catch (InvalidOperationException)
                {
                    VRChatifyUtils.DebugLog("No session found");
                    return null;
                }
            }
            return currentSession;
        }
        HttpClient client = new HttpClient();
        public string GetSongName()
        {

            var songInfo = GetCurrentSession().ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
              
                return songInfo.Title;
            }
            return "Unable to get Lyrics";
        }
      
       public static Dictionary<TimeSpan, string> dic = new Dictionary<TimeSpan, string>();
        static string syncedlyrics = "";
        static string ConvertJsonToString(string json)
        {
            try
            {
                JObject jsonObject = JObject.Parse(json);

                string trackName = jsonObject["trackName"].ToString();
                string artistName = jsonObject["artistName"].ToString();
                string albumName = jsonObject["albumName"].ToString();
                int duration = jsonObject["duration"].Value<int>();
                bool instrumental = jsonObject["instrumental"].Value<bool>();
                string plainLyrics = jsonObject["plainLyrics"].ToString();
                syncedlyrics = jsonObject["syncedLyrics"].ToString();

                // Construct the final string
                string result = $"Track: {trackName}\nArtist: {artistName}\nAlbum: {albumName}\nDuration: {duration} seconds\nInstrumental: {(instrumental ? "Yes" : "No")}";
                return result;
            }
            catch (Exception ex)
            {
                return $"Error converting JSON: {ex.Message}";
            }
        }
        public static TimeSpan convertthing(string timespan)
        {
            string s = timespan;
            if (s.Contains('.'))
            {
              // VRChatifyUtils.DebugLog(s.IndexOf('.').ToString());
                s = s.Substring(0, s.IndexOf("."));
            //    VRChatifyUtils.DebugLog(s);
            }
            string[] val = s.Split(':');
         //   VRChatifyUtils.DebugLog(int.Parse(val[0]) + "\n" + int.Parse(val[1]));

            TimeSpan ts = new TimeSpan(00, int.Parse(val[0]), int.Parse(val[1]));
            return ts;
        }
        public async Task<string> GetLyrics()
        {
            var songInfo = GetCurrentSession().ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
                dic.Clear();
                var ly = await client.GetAsync($"https://lrclib.net/api/get?artist_name={songInfo.Artist.Replace(' ', '+')}&track_name={songInfo.Title.Replace(' ', '+')}&album_name={songInfo.AlbumTitle.Replace(' ', '+')}&duration={GetSongDuration().TotalSeconds}");
               // Task.Delay(1000);
                if (ly.IsSuccessStatusCode)
                {
                    
                    var s = await ly.Content.ReadAsStringAsync();
                    ConvertJsonToString(s);
                   // Console.WriteLine(s);
                    if (s.Contains("["))
                    {

                        foreach (var idk in syncedlyrics.Split('\n'))
                        {
                            var subidk = idk.Substring(idk.IndexOf("["), idk.IndexOf("]")).Replace("[", string.Empty).Replace("]", string.Empty);
                            var ts = convertthing(subidk);
                            dic.Add(ts, idk.Split(']')[1]);


                        }
                        var dif = dic.OrderBy(comp => Math.Abs(comp.Key.TotalSeconds - GetCurrentSongTime().TotalSeconds)).FirstOrDefault();
                        var outstr = "";
                        dic.TryGetValue(dif.Key, out outstr);
                        VRChatifyUtils.DebugLog(outstr);

                        return outstr;
                    }
                }
            }
            return "Unable to get Lyrics";

        }
        public string GetSongArtist()
        {
            var songInfo = GetCurrentSession()?.ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
                return songInfo.Artist;
            }
            return "Unable to get Author";
        }

        public string GetAlbumTitle()
        {
            var songInfo = GetCurrentSession()?.ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
                return songInfo.AlbumTitle;
            }
            return "Unable to get Author";
        }

        public TimeSpan GetSongDuration()
        {
            var timeline = GetCurrentSession()?.ControlSession.GetTimelineProperties();
            if(timeline != null)
            {
                return timeline.EndTime;
            }
            return new TimeSpan();
        }

        public TimeSpan GetCurrentSongTime()
        {
            var timeline = GetCurrentSession()?.ControlSession.GetTimelineProperties();
            if (timeline != null)
            {
                return timeline.Position;
            }
            return new TimeSpan();
        }

        public string GetAlbumTrackCount()
        {
            var songInfo = GetCurrentSession()?.ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
                return songInfo.AlbumTrackCount.ToString();
            }
            return "Unable to get Author";
        }

        public MediaManager GetMediaManager()
        {
            return mediaManager;
        }

        public void setCurrentSession(MediaManager.MediaSession session)
        {
            currentSession = session;
        }
    }
}
