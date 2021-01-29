using System;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoUI
{
    public class AlertManager : MonoBehaviour
    {
        [SerializeField] private ModalWindowManager _defaultModal;
        [SerializeField] private ModalWindowManager _errorModal;

        public static AlertManager Instance;

        private readonly UnityEvent _onConfirm = new UnityEvent();

        private void Awake()
        {
            Instance = this;
            _defaultModal.onConfirm.AddListener(() =>
            {
                _onConfirm?.Invoke();
                _onConfirm?.RemoveAllListeners();
            });
        }

        public void  ShowDefault(string title, string message, Action onConfirm = null)
        {
            OpenModal(_defaultModal, title, message);
            if (onConfirm != null)
            {
                _onConfirm.AddListener(() => { onConfirm(); });
            }
        }

        public void ShowError(string titleKey, string messageKey)
        {
            OpenModal(_errorModal, I2.Loc.LocalizationManager.GetTranslation(titleKey),
                I2.Loc.LocalizationManager.GetTranslation(messageKey));
        }

        private void OpenModal(ModalWindowManager modalWindow, string title, string message)
        {
            modalWindow.CloseWindow();
            
            modalWindow.titleText = title;

            modalWindow.descriptionText = message;

            modalWindow.UpdateUI();
            modalWindow.OpenWindow();
        }
    }
}