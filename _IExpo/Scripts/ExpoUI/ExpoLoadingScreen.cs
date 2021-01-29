using System;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoLoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        public static ExpoLoadingScreen Instance;

        private void Awake()
        {
            Instance = this;
            _loadingScreen.SetActive(false);
        }

        public void Show()
        {
            _loadingScreen?.SetActive(true);
        }

        public void Hide()
        {
            _loadingScreen?.SetActive(false);
        }
    }
}