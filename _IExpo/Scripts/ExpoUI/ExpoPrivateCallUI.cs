using System;
using Photon.Realtime;
using SpaceHub.Conference;
using TMPro;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoPrivateCallUi : MonoBehaviour
    {
        [SerializeField] private GameObject _callPanel;
        [SerializeField] private TextMeshProUGUI _callerName;

        public static ExpoPrivateCallUi Instance;

        private Action _onLeaveClick;

        private void Awake()
        {
            Instance = this;
            Hide();
        }

        public void SetOnLeaveClick(Action action)
        {
            _onLeaveClick = action;
        }

        public void OnLeaveClick()
        {
            _onLeaveClick?.Invoke();
            Hide();
        }

        public void Hide()
        {
            _callPanel.SetActive(false);
        }

        public void Show()
        {
            _callPanel.SetActive(true);
        }
    }
}