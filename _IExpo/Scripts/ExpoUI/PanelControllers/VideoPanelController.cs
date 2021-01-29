using System;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class VideoPanelController : BasePanelController, IEventSubscribable<VideoPanelEventType>
    {
        private event Action<VideoPanelEventType> _controlEvent;

        public static VideoPanelController Instance;
        [SerializeField] private GameObject _loadingBar;
        [SerializeField] private VideoProgressBar _progressBar;
        [SerializeField] private Slider _speedSlider;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private PauseIcon _pauseIcon;
        private YoutubePlayer _currentPlayer;

        protected override void AfterAwake()
        {
            Instance = this;
            HideLoadingScreen();
        }

        public void SetupPlayer(YoutubePlayer player)
        {
            _currentPlayer = player;
            _pauseIcon.p = _currentPlayer;

            _currentPlayer.volumeSlider = _volumeSlider;
            _currentPlayer.playbackSpeed = _speedSlider;
            _progressBar.player = player;
            _currentPlayer.progress = _progressBar.gameObject.GetComponent<Image>();
            _currentPlayer.progress.fillAmount = 0f;
        }

        public void OnVolumeClick()
        {
            Debug.Assert(_currentPlayer != null, nameof(_currentPlayer) + " != null");
            _currentPlayer.VolumeSlider();
        }

        public void OnSpeedClick()
        {
            Debug.Assert(_currentPlayer != null, nameof(_currentPlayer) + " != null");
            _currentPlayer.PlaybackSpeedSlider();
        }

        public void OnSpeedChange()
        {
            Debug.Assert(_currentPlayer != null, nameof(_currentPlayer) + " != null");
            _currentPlayer.Speed();
        }

        public void OnVolumeChange()
        {
            _currentPlayer.Volume();
        }

        public void ShowLoadingScreen()
        {
            _loadingBar?.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            _loadingBar?.SetActive(false);
        }

        public void Subscribe(Action<VideoPanelEventType> action)
        {
            _controlEvent += action;
        }

        public void Unsubscribe(Action<VideoPanelEventType> action)
        {
            _controlEvent -= action;
        }

        public void OnCloseClick()
        {
            _controlEvent?.Invoke(VideoPanelEventType.Close);
            SwitchTo(ExpoUIType.InGame);
        }

        public void OnPauseClick()
        {
            _controlEvent?.Invoke(VideoPanelEventType.Pause);
        }
    }

    public enum VideoPanelEventType
    {
        Pause,
        Close
    }
}