using System;
using System.Linq;
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

        public string GetSongName()
        {
            var songInfo = GetCurrentSession().ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
                return songInfo.Title;
            }
            return "Unable to get Title";
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
