using System;
using _IExpo.Scripts.ExpoUI;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace _IExpo.Scripts
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private ExpoCharacterCustomizationUI _expoCharacterCustomizationUI;

        [SerializeField] private GameObject _panel;
        [SerializeField] private YoutubePlayer _player;
        [SerializeField] private GameObject _skipButton;

        [SerializeField] private float _skipTimeoutSec = 5f;

        private const string TAG = "tutorialShown";

        private void Start()
        {
            _player.OnVideoFinished.AddListener(OnTutorialFinished);
        }

        public void StartVideo()
        {
#if UNITY_IOS || UNITY_ANDROID
            _player.Play(LocalizationManager.GetTermTranslation("Lobby/AvatarSettings/TutorialMobileURL"));
#else
            _player.Play(LocalizationManager.GetTermTranslation("Lobby/AvatarSettings/TutorialURL"));
#endif

            _panel.SetActive(true);

            StartCoroutine(ShowSkipButtonRoutine());
        }

        private IEnumerator ShowSkipButtonRoutine()
        {
            yield return new WaitForSeconds(_skipTimeoutSec);

            _skipButton.SetActive(true);
        }

        public void SkipVideo()
        {
            StopAllCoroutines();

            StartCoroutine(SkipVideoRoutine());
        }

        private IEnumerator SkipVideoRoutine()
        {
            _player.Stop();
            _skipButton.SetActive(false);

            yield return new WaitForSeconds(0.1f);
            ViewOverlay.Instance.EnableFor(1f);

            _expoCharacterCustomizationUI.OnJoinClick();
        }

        private void OnTutorialFinished()
        {
            _expoCharacterCustomizationUI.OnJoinClick();
        }
    }
}