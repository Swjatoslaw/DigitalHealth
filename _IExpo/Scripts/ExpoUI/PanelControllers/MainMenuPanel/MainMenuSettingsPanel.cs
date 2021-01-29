using System.Collections;
using System.Collections.Generic;
using _IExpo.Scripts.ExpoLocalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.Unity;
using SpaceHub.Conference;

namespace _IExpo.Scripts.ExpoUI.PanelControllers.MainMenuPanel
{
    public class MainMenuSettingsPanel : MonoBehaviour
    {
        public Slider MasterVolume;

        public Slider InputVolume;

        Recorder m_Recorder
        {
            get { return ConferenceVoiceConnection.Instance?.PrimaryRecorder; }
        }

        public AudioSource SoundcheckAudio;

        private const string EnglishLanguage = "English";
        private const string RussianLanguage = "Russian";

        private void OnEnable()
        {
            if (AudioSettingsManager.Instance != null)
            {
                if (MasterVolume.onValueChanged.GetPersistentEventCount() > 0)
                {
                    MasterVolume.onValueChanged.RemoveListener(AudioSettingsManager.Instance.SetMasterVolume);
                }

                MasterVolume.value = AudioSettingsManager.Instance.MasterVolume;

                MasterVolume.onValueChanged.AddListener(AudioSettingsManager.Instance.SetMasterVolume);

                // Microphone
                if (m_Recorder == null)
                {
                    return;
                }

                if (InputVolume.onValueChanged.GetPersistentEventCount() > 0)
                {
                    InputVolume.onValueChanged.RemoveListener(OnInputVolumeChanged);
                }

                var amplifier = m_Recorder.GetComponent<Photon.Voice.Unity.UtilityScripts.MicAmplifier>();
                if (amplifier != null)
                {
                    float inputVolume = PlayerPrefs.GetFloat("InputVolume", 1f);
                    amplifier.AmplificationFactor = inputVolume;

                    InputVolume.value = PlayerPrefs.GetFloat("InputVolume", 1f);
                }

                InputVolume.onValueChanged.AddListener(OnInputVolumeChanged);
            }
        }

        void OnInputVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("InputVolume", value);

            var amplifier = m_Recorder.GetComponent<Photon.Voice.Unity.UtilityScripts.MicAmplifier>();

            if (amplifier != null)
            {
                amplifier.AmplificationFactor = value;
            }
        }

        public void OnRuClicked()
        {
            ApplyLanguage(RussianLanguage);
        }

        public void OnEnClicked()
        {
            ApplyLanguage(EnglishLanguage);
        }

        private void ApplyLanguage(string language)
        {
            AlertManager.Instance.ShowDefault(I2.Loc.LocalizationManager.GetTranslation("GamePanel/LanguageChangeTitle"),
                $"{I2.Loc.LocalizationManager.GetTranslation("GamePanel/LanguageChangeMessage1")} {language}. {I2.Loc.LocalizationManager.GetTranslation("GamePanel/LanguageChangeMessage2")}",
                () =>
                {
                    ExpoLocalizationManager.Instance.ApplyLanguage(language);
                    ConferenceRoomManager.LoadRoom(ConferenceRoomManager.Instance.CurrentRoom);
                });
        }

        public void OnOutputVolumeDragEnd()
        {
            SoundcheckAudio.Play();
        }
    }
}