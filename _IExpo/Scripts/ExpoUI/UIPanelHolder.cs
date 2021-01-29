using System;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    [Serializable]
    public class UIPanelHolder
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private ExpoUIType _uiType;
        [SerializeField] private ExpoProjectPlatform _platform;

        public ExpoUIType UiType => _uiType;
        public ExpoProjectPlatform Platform => _platform;

        public void Disable()
        {
            if (_panel != null)
            {
                _panel.SetActive(false);
            }
            else
            {
                Debug.LogError($"No gameobject panel for: {_uiType} {_platform}");
            }
        }

        public void Enable()
        {
            if (_panel != null)
            {
                _panel.SetActive(true);
            }
            else
            {
                Debug.LogError($"No gameobject panel for: {_uiType} {_platform}");
            }
        }
    }
}