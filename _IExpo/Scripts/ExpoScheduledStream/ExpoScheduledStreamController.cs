using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace _IExpo.Scripts.ExpoScheduledStream
{
    public class ExpoScheduledStreamController : MonoBehaviour
    {
        [Header("Settings")] public string StreamRoomName;

        public bool autoStart = true;
        [Range(12f, 120f)] public float internetTimerUpdatePeriod = 60f;
        [Range(12f, 120f)] public float scheduledStreamUpdatePeriod = 0.5f;

        [Header("Links")] public YoutubePlayer mPlayer;

        public TextMeshPro streamStartsText;
        public TextMeshPro streamStartsTimerText;

        [Header("Current data")] [SerializeField]
        private DateTime internetTimeCached;

        [SerializeField] private string internetTimeCachedString;

        [SerializeField] private bool internetTimeReady;

        [SerializeField] private float internetTimeCachedMoment;
        [SerializeField] private DateTime currentTime;
        [SerializeField] private string currentTimeString;

        [SerializeField] private ExpoScheduledStreamState _streamState;

        public ExpoScheduledStreamState StreamState
        {
            get { return _streamState; }
            private set { _streamState = value; }
        }

        [SerializeField] private bool _isDirtyStreams;

        private ExpoScheduledStreamItem currentStreamItem;

        private ExpoScheduledStreamItem nextStreamItem;

        private ExpoScheduledStreamItem activeStreamItem;
        [SerializeField] private float activeStreamDurationSec;
        [SerializeField] private int timeBeforeNextStreamSec;

        private Coroutine scheduleMonitorCoroutine;
        private bool internetTimerIsActive = false;

        private Coroutine scheduledStreamCoroutine;
        private bool scheduledStreamIsActive = false;

        VideoPlayer.EventHandler prepareHandler = null;
        VideoPlayer.FrameReadyEventHandler frameReadyHandler = null;
        VideoPlayer.FrameReadyEventHandler seekReadyHandler = null;

        void Start()
        {
            _isDirtyStreams = true;

            internetTimeReady = false;
            internetTimerIsActive = false;
            scheduledStreamIsActive = false;

            StreamState = ExpoScheduledStreamState.Start;

            StreamRoomName = ExpoScheduledStreamData.RoomName;

            if (autoStart)
            {
                StartInternetTimer();
                StartScheduledStream();
            }

            if (mPlayer != null)
            {
                // AVPro player
                //mPlayer.Events.AddListener(OnVideoEvent);

                // YouTubePlayer
                prepareHandler = (source) => { OnReadyToStart(); };
                mPlayer.videoPlayer.prepareCompleted += prepareHandler;

                mPlayer.OnVideoFinished.AddListener(OnFinished);
            }
        }

        private void OnDestroy()
        {
            if (mPlayer != null)
            {
                // YouTubePlayer
                mPlayer.OnVideoReadyToStart.RemoveAllListeners();

                mPlayer.videoPlayer.prepareCompleted -= prepareHandler;

                mPlayer.videoPlayer.frameReady -= frameReadyHandler;

                mPlayer.OnVideoFinished.RemoveAllListeners();
            }
        }


        public void StartInternetTimer()
        {
            internetTimeReady = false;

            if (internetTimerIsActive)
            {
                StopInternetTimer();
            }

            scheduleMonitorCoroutine = StartCoroutine(InternetTimerCoroutine());
        }

        public void StopInternetTimer()
        {
            internetTimerIsActive = false;
            internetTimeReady = false;

            if (scheduleMonitorCoroutine != null)
            {
                StopCoroutine(scheduleMonitorCoroutine);
            }
        }

        private IEnumerator InternetTimerCoroutine()
        {
            internetTimerIsActive = true;

            while (internetTimerIsActive)
            {
                internetTimeReady = false;
                yield return GetInternetTime();
                internetTimeReady = true;

                yield return new WaitForSecondsRealtime(internetTimerUpdatePeriod);
            }
        }

        public IEnumerator GetInternetTime()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get("http://www.google.com"))
            {
                webRequest.timeout = 10;
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    internetTimeCached = DateTime.Now; //In case something goes wrong. 
                }
                else
                {
                    internetTimeCached = DateTime.ParseExact(webRequest.GetResponseHeaders()["date"],
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AssumeUniversal);
                }

                internetTimeCachedMoment = Time.realtimeSinceStartup;

                internetTimeCachedString = internetTimeCached.ToString();
            }
        }

        private void UpdateCurrentNextStreamItems()
        {
            ExpoScheduledStreamItem
                newCurrentStreamItem =
                    ExpoScheduleManager.Instance.GetCurrentScheduledStreamItem(StreamRoomName, currentTime);
            ExpoScheduledStreamItem
                newNextStreamItem =
                    ExpoScheduleManager.Instance.GetNextScheduledStreamItem(StreamRoomName, currentTime);

            if (newCurrentStreamItem != currentStreamItem || newNextStreamItem != nextStreamItem)
            {
                currentStreamItem = newCurrentStreamItem;
                nextStreamItem = newNextStreamItem;

                _isDirtyStreams = true;
            }
        }

        public void StartScheduledStream()
        {
            if (scheduledStreamIsActive)
            {
                StopScheduledStream();
            }

            scheduledStreamCoroutine = StartCoroutine(ScheduledStreamCoroutine());
        }

        public void StopScheduledStream()
        {
            scheduledStreamIsActive = false;

            if (scheduledStreamCoroutine != null)
            {
                StopCoroutine(scheduledStreamCoroutine);
            }
        }

        private IEnumerator ScheduledStreamCoroutine()
        {
            scheduledStreamIsActive = true;

            activeStreamItem = null;

            while (scheduledStreamIsActive)
            {
                while (!internetTimeReady)
                {
                    yield return new WaitForSecondsRealtime(0.5f);
                }

                currentTime = internetTimeCached.AddSeconds(Time.realtimeSinceStartup - internetTimeCachedMoment);
                currentTimeString = currentTime.ToString();

                UpdateCurrentNextStreamItems();

                if (_isDirtyStreams)
                {
                    activeStreamItem = null;
                    StreamState = ExpoScheduledStreamState.Start;
                }

                if (StreamState == ExpoScheduledStreamState.Start)
                {
                    if (currentStreamItem != null)
                    {
                        activeStreamItem = currentStreamItem;
                        OpenStreamFile();
                    }
                    else
                    {
                        OnFinished();
                    }
                }
                else if (StreamState == ExpoScheduledStreamState.Ready)
                {
                    if (currentTime >= activeStreamItem.StartTime)
                    {
                        float timeStampSec = (float) currentTime.Subtract(activeStreamItem.StartTime).TotalSeconds;

                        if (timeStampSec < activeStreamDurationSec)
                        {
                            PlayStreamFile(timeStampSec);
                        }
                        else
                        {
                            OnFinished();
                        }
                    }
                }
                else if (StreamState == ExpoScheduledStreamState.Playing)
                {
                }
                else if (StreamState == ExpoScheduledStreamState.Waiting)
                {
                    // next stream always not null
                    if (activeStreamItem != null)
                    {
                        if (activeStreamItem != nextStreamItem)
                        {
                            activeStreamItem = nextStreamItem;
                            OpenStreamFile();
                        }
                        else
                        {
                            StreamState = ExpoScheduledStreamState.Ready;
                        }
                    }
                    else
                    {
                        activeStreamItem = nextStreamItem;
                        OpenStreamFile();
                    }
                }
                else if (StreamState == ExpoScheduledStreamState.Over)
                {
                    activeStreamItem = null;

                    CloseStreamFile();
                }
                else if (StreamState == ExpoScheduledStreamState.WaitAcyncOperation)
                {
                }

                _isDirtyStreams = false;


                timeBeforeNextStreamSec = 0;
                if (nextStreamItem != null)
                {
                    timeBeforeNextStreamSec =
                        Mathf.RoundToInt((float) nextStreamItem.StartTime.Subtract(currentTime).TotalSeconds);
                }

                UpdateText();

                yield return new WaitForSecondsRealtime(scheduledStreamUpdatePeriod);
            }

            yield return null;
        }

        private void UpdateText()
        {
            switch (StreamState)
            {
                case ExpoScheduledStreamState.Start:
                    streamStartsText.text = "Start";
                    streamStartsTimerText.text = " Start";
                    break;
                case ExpoScheduledStreamState.Ready:
                    if (nextStreamItem != null)
                    {
                        streamStartsText.text = "(Ready) Next:";
                        streamStartsTimerText.text = timeBeforeNextStreamSec.ToString() + " sec";
                    }

                    break;
                case ExpoScheduledStreamState.Playing:
                    if (nextStreamItem != null)
                    {
                        streamStartsText.text = "(Playing) Next:";
                        streamStartsTimerText.text = timeBeforeNextStreamSec.ToString() + " sec";
                    }
                    else
                    {
                        streamStartsText.text = "(Playing) Next:";
                        streamStartsTimerText.text = "No";
                    }

                    break;
                case ExpoScheduledStreamState.Waiting:
                    streamStartsText.text = "(Waiting) Next:";
                    streamStartsTimerText.text = timeBeforeNextStreamSec.ToString() + " sec";
                    break;
                case ExpoScheduledStreamState.Over:
                    streamStartsText.text = "End";
                    streamStartsTimerText.text = "End";
                    break;
                default:
                    break;
            }
        }

        private void OnReadyToStart()
        {
            activeStreamDurationSec = (mPlayer.videoPlayer.frameCount / mPlayer.videoPlayer.frameRate);

            mPlayer.Pause();

            frameReadyHandler = (source, index) => { OnFrameReady(); };
            mPlayer.videoPlayer.frameReady += frameReadyHandler;
            mPlayer.videoPlayer.sendFrameReadyEvents = true;
        }

        private void OnFrameReady()
        {
            activeStreamDurationSec = (mPlayer.videoPlayer.frameCount / mPlayer.videoPlayer.frameRate);

            mPlayer.videoPlayer.frameReady -= frameReadyHandler;
            mPlayer.videoPlayer.sendFrameReadyEvents = false;

            mPlayer.Pause();

            StreamState = ExpoScheduledStreamState.Ready;
        }

        private void OnStarted()
        {
            StreamState = ExpoScheduledStreamState.Playing;
        }

        private void OnFinished()
        {
            if (nextStreamItem != null)
            {
                StreamState = ExpoScheduledStreamState.Waiting;
            }
            else
            {
                StreamState = ExpoScheduledStreamState.Over;
            }
        }

        private void OpenStreamFile()
        {
            StreamState = ExpoScheduledStreamState.WaitAcyncOperation;

            if (activeStreamItem != null)
            {
                mPlayer.Play(activeStreamItem.URL);
            }
            else
            {
                StreamState = ExpoScheduledStreamState.Start;
            }
        }

        private void PlayStreamFile(float timeStampSec)
        {
            StreamState = ExpoScheduledStreamState.WaitAcyncOperation;

            mPlayer.Seek(timeStampSec);

            seekReadyHandler = (source, index) => { OnSeekReady(); };

            mPlayer.videoPlayer.sendFrameReadyEvents = true;
            mPlayer.videoPlayer.frameReady += seekReadyHandler;
        }

        private void OnSeekReady()
        {
            mPlayer.videoPlayer.sendFrameReadyEvents = false;
            mPlayer.videoPlayer.frameReady -= seekReadyHandler;

            mPlayer.Play();

            StreamState = ExpoScheduledStreamState.Playing;
        }

        private void CloseStreamFile()
        {
            mPlayer.Stop();
        }
    }
}