using UnityEngine;

namespace _IExpo.Scripts.ExpoScheduledStream
{
    [System.Serializable]
    public class ExpoScheduledStreamItem
    {
        public int StreamId;

        public string Room;
        public int RoomId;

        public int DayId;

        public System.DateTime Date;
        public string DateString;
        
        public System.DateTime StartTime;
        public string StartTimeString;
        public System.DateTime EndTime;
        public string EndTimeString;

        public string Title;
        public string Description;

        public string SpeakersName;
        public string SpeakersPosition;
        public string SpeakersPhoto;

        public string Format;

        public string URL;

        [Header("Localization term")]
        public string category;
    }
}