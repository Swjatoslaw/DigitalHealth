using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoCopyToClipboardScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _copyToClipboardScreen;
        [SerializeField] private NotificationManager _notification;

        public static ExpoCopyToClipboardScreen Instance;

        private void Awake()
        {
            Instance = this;
            _copyToClipboardScreen.SetActive(false);
        }
        public void Show()
        {
            _copyToClipboardScreen?.SetActive(true);

            StartCoroutine(ShowRoutine());
        }
        private IEnumerator ShowRoutine()
        {
            _notification.OpenNotification();
            yield return new WaitForSecondsRealtime(_notification.timer + 0.25f);

            Hide();
        }
        public void Hide()
        {
            _copyToClipboardScreen?.SetActive(false);
        }
    }
}