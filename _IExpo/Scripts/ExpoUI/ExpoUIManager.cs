using _IExpo.Scripts.ExpoCharacter;
using SpaceHub.Conference;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoUIManager : MonoBehaviour
    {
        public static ExpoUIManager Instance;

        [SerializeField] private UIPanelHolder[] _panelHolders;
        private UIPanelHolder _current;

        [Header("Runtime Variables")] [SerializeField]
        private ExpoProjectPlatform _currentPlatform;

        [SerializeField] private ExpoRig _expoRig;

        private void Awake()
        {
            Instance = this;

            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene arg0, LoadSceneMode mode)
        {
            if (mode != LoadSceneMode.Additive)
            {
                return;
            }

            CheckViewTModeAndInput();
        }

        private void CheckViewTModeAndInput()
        {
            var selectedViewMode = ViewModeManager.Instance.GetSelectedViewMode();

            if (selectedViewMode == ViewModeManager.ViewMode.Mobile)
            {
                _currentPlatform = ExpoProjectPlatform.Mobile;
            }
            else
            {
                _currentPlatform = ExpoProjectPlatform.PC;
            }

            _expoRig = FindObjectOfType<ExpoRig>();

            if (_expoRig != null && _expoRig.isActiveAndEnabled)
            {
                _expoRig.SetLockInput(false);
            }
        }

        private IEnumerator Start()
        {
            _currentPlatform = ExpoProjectPlatform.PC;

            var current = ViewModeManager.Instance.CurrentViewModeType;

            foreach (UIPanelHolder uiPanelHolder in _panelHolders)
            {
                uiPanelHolder.Disable();

                if (current == ViewModeManager.ViewMode.ThirdPerson &&
                    (uiPanelHolder.Platform == ExpoProjectPlatform.PC ||
                     uiPanelHolder.Platform == ExpoProjectPlatform.Universal))
                {
                    uiPanelHolder.Enable();
                }

                if (current == ViewModeManager.ViewMode.Mobile &&
                    (uiPanelHolder.Platform == ExpoProjectPlatform.Mobile ||
                     uiPanelHolder.Platform == ExpoProjectPlatform.Universal))
                {
                    uiPanelHolder.Enable();
                }
            }

            while (!PlayerLocal.Instance.Client.IsConnectedAndReady)
            {
                yield return null;
            }

            foreach (UIPanelHolder uiPanelHolder in _panelHolders)
            {
                uiPanelHolder.Disable();
            }

            CheckViewTModeAndInput();

            ShowUI(ExpoUIType.InGame);
        }

        public void ShowUI(ExpoUIType uiType)
        {
            var panel = _panelHolders.FirstOrDefault(a =>
                a.UiType == uiType && (a.Platform == _currentPlatform || a.Platform == ExpoProjectPlatform.Universal));

            if (panel != null)
            {
                foreach (UIPanelHolder uiPanelHolder in _panelHolders)
                {
                    uiPanelHolder.Disable();
                }

                panel.Enable();
                _current = panel;

                if (panel.UiType == ExpoUIType.InGame)
                {
                    Invoke(nameof(DelayedSetUnlockInput), 0.1f);
                }
                else
                {
                    if (_expoRig != null && _expoRig.isActiveAndEnabled)
                    {
                        _expoRig.SetLockInput(true);
                    }
                }
            }
        }

        private void DelayedSetUnlockInput()
        {
            if (_expoRig != null && _expoRig.isActiveAndEnabled)
            {
                _expoRig.SetLockInput(false);
            }
        }
    }

    public enum ExpoProjectPlatform
    {
        PC,
        Mobile,
        Universal
    }
}