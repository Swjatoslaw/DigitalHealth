using _IExpo.Scripts.ExpoLocalization;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _IExpo.Scripts.ExpoScheduledStream
{
    [System.Serializable]
    public struct ExpoScheduleDay
    {
        public int DayId;

        public System.DateTime Date;
        public string DateString;
    }


    public class ExpoScheduleManager : MonoBehaviour
    {
        public static ExpoScheduleManager Instance;
        
        public string categoryMainName = "StreamSchedule";
        public string categoryStreamName = "Stream_";

        public string keyRoom = "Room";
        public string keyDayId = "DayId";

        public string keyStartTime = "StartTime";
        public string keyEndTime = "EndTime";

        public string keyTitle = "Title";
        public string keyDescription = "Description";

        public string keySpeakersName = "SpeakersName";
        public string keySpeakersPosition = "SpeakersPosition";
        public string keySpeakersPhoto = "SpeakersPhoto";

        public string keyFormat = "Format";

        public string keyURL = "URL";

        public string dateFormat = "ddd MMM dd yyyy HH:mm:ss zzz";

        [SerializeField] private List<string> categoriesList = new List<string>();
        [SerializeField] private List<string> streamCategoriesList = new List<string>();

        [SerializeField] private List<string> termsList = new List<string>();

        [SerializeField] private List<ExpoScheduledStreamItem> scheduledItems = new List<ExpoScheduledStreamItem>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ExpoLocalizationManager.Instance.OnLanguageChanged += ParseSchedule;

            StartCoroutine(DelayedParseSchedule());
        }

        private void OnDestroy()
        {
            ExpoLocalizationManager.Instance.OnLanguageChanged -= ParseSchedule;
        }

        private IEnumerator DelayedParseSchedule()
        {
            while (I2.Loc.LocalizationManager.Sources.Count == 0)
            {
                yield return null;
            }
            
            ParseSchedule();
        }

        private void ParseSchedule()
        {
            LanguageSourceData sourceData = I2.Loc.LocalizationManager.Sources[0];

            categoriesList = new List<string>();
            categoriesList = sourceData.GetCategories();
            streamCategoriesList = categoriesList.FindAll(x => x.StartsWith(categoryMainName));

            foreach (string streamCategory in streamCategoriesList)
            {
                termsList.AddRange(sourceData.GetTermsList(streamCategory));
            }

            termsList.Sort();

            string key;
            string category;
            
            foreach (string term in termsList)
            {
                key = "";
                category = "";

                LanguageSourceData.DeserializeFullTerm(term, out key, out category);

                string[] subCategories = category.Split('/');

                int streamId = 0;
                
                foreach (string item in subCategories)
                {
                    if (item.StartsWith(categoryStreamName))
                    {
                        int.TryParse(item.Substring(categoryStreamName.Length), out streamId);
                    }
                }

                ExpoScheduledStreamItem scheduledItem = scheduledItems.Find(x => x.category == category);
                bool isNew = false;
                if (scheduledItem == null)
                {
                    isNew = true;

                    scheduledItem = new ExpoScheduledStreamItem();

                    scheduledItem.StreamId = streamId;
                    scheduledItem.category = category;
                }

                if (key == keyRoom)
                {
                    // Room name is equal to Timetable Tab names
                    scheduledItem.Room = sourceData.GetTranslation(term);
                }
                else if (key == keyDayId)
                {
                    int dayIdParse = 0;
                    if (!int.TryParse(sourceData.GetTranslation(term), out dayIdParse))
                    {
                        dayIdParse = 0;
                    }
                    
                    scheduledItem.DayId = dayIdParse;
                }
                else if (key == keyStartTime)
                {
                    string dateString = sourceData.GetTranslation(term);
                    
                    dateString = dateString.Replace("GMT", "").Replace(" (PDT)", "").Replace(" (PST)", "").Replace(" (PT)", "");
                    dateString = dateString.Insert(dateString.Length - 2, ":");
                    
                    scheduledItem.StartTime = System.DateTime.ParseExact(dateString, dateFormat, System.Globalization.CultureInfo.InvariantCulture);
                    scheduledItem.StartTimeString = scheduledItem.StartTime.ToString();

                    scheduledItem.Date = scheduledItem.StartTime.ToLocalTime().Date;
                    scheduledItem.DateString = scheduledItem.Date.ToShortDateString();
                }
                else if (key == keyEndTime)
                {
                    string dateString = sourceData.GetTranslation(term);

                    dateString = dateString.Replace("GMT", "").Replace(" (PDT)", "").Replace(" (PST)", "").Replace(" (PT)", "");
                    dateString = dateString.Insert(dateString.Length - 2, ":");

                    scheduledItem.EndTime = System.DateTime.ParseExact(dateString, dateFormat, System.Globalization.CultureInfo.InvariantCulture);
                    scheduledItem.EndTimeString = scheduledItem.EndTime.ToString();
                }
                else if (key == keyTitle)
                {
                    scheduledItem.Title = sourceData.GetTranslation(term);
                }
                else if (key == keyDescription)
                {
                    scheduledItem.Description = sourceData.GetTranslation(term);
                }
                else if (key == keySpeakersName)
                {
                    scheduledItem.SpeakersName = sourceData.GetTranslation(term);
                }
                else if (key == keySpeakersPosition)
                {
                    scheduledItem.SpeakersPosition = sourceData.GetTranslation(term);
                }
                else if (key == keySpeakersPhoto)
                {
                    scheduledItem.SpeakersPhoto = sourceData.GetTranslation(term);
                }
                else if (key == keyFormat)
                {
                    scheduledItem.Format = sourceData.GetTranslation(term);
                }
                else if (key == keyURL)
                {
                    scheduledItem.URL = sourceData.GetTranslation(term);
                }
                
                if (isNew)
                {
                    scheduledItems.Add(scheduledItem);
                }

            }
            
            scheduledItems = scheduledItems.OrderBy(x => x.StartTime).ToList();
        }
        
        public ExpoScheduledStreamItem GetCurrentScheduledStreamItem(string steamRoomName, DateTime currentTime)
        {
            List<ExpoScheduledStreamItem> todaySchedule = scheduledItems.FindAll(x => (x.Room == steamRoomName && !string.IsNullOrEmpty(x.URL)));

            todaySchedule = todaySchedule.OrderBy(x => x.StartTime).ToList();

            return todaySchedule.FindLast(x => x.StartTime <= currentTime);
        }

        public ExpoScheduledStreamItem GetNextScheduledStreamItem(string steamRoomName, DateTime currentTime)
        {
            List<ExpoScheduledStreamItem> todaySchedule = scheduledItems.FindAll(x => (x.Room == steamRoomName && !string.IsNullOrEmpty(x.URL)));
            
            todaySchedule = todaySchedule.OrderBy(x => x.StartTime).ToList();

            return todaySchedule.Find(x => x.StartTime > currentTime);
        }

        public List<ExpoScheduleDay> GetExpoScheduleDays()
        {
            if(scheduledItems.Count == 0)
            {
                ParseSchedule();
            }

            List<ExpoScheduleDay> daysList = new List<ExpoScheduleDay>();

            int currentDayId = -1;

            foreach(ExpoScheduledStreamItem item in scheduledItems)
            {
                if (item.DayId > currentDayId)
                {
                    currentDayId = item.DayId;

                    ExpoScheduleDay newDay = new ExpoScheduleDay
                    {
                        DayId = item.DayId,
                        Date = item.Date,
                        DateString = item.DateString
                    };

                    daysList.Add(newDay);
                }
                else
                {
                    continue;
                }
            }

            return daysList;
        }

        public List<ExpoScheduledStreamItem> GetDayScheduledItems(int day)
        {
            List<ExpoScheduledStreamItem> daySchedule = scheduledItems.FindAll(x => x.DayId == day);

            daySchedule = daySchedule.OrderBy(x => x.StartTime).ToList();

            return daySchedule;
        }

        public List<ExpoScheduledStreamItem> GetAllScheduledItems()
        {
            List<ExpoScheduledStreamItem> allSchedule = scheduledItems.OrderBy(x => x.StartTime).ToList();

            return allSchedule;
        }
    }
}