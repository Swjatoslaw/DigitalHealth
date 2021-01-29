using System;
using System.Collections;
using Michsky.UI.ModernUIPack;
using SpaceHub.Conference;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoNotificationStarter : MonoBehaviour
    {
        [SerializeField] private NotificationManager _notificationManager;

        private IEnumerator Start()
        {
            yield return null;
            _notificationManager.OpenNotification();
        }
    }
}