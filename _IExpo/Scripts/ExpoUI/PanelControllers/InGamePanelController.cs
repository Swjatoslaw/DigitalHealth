using System;
using System.Collections.Generic;
using System.Linq;
using _IExpo.Scripts.ExpoPrivateChat;
using _IExpo.Scripts.ExpoUtils;
using SpaceHub.Conference;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class InGamePanelController : BasePanelController
    {
        [SerializeField] private GameObject _chatButton;
        [SerializeField] private ChatMessagesNotificationController _newMessagesObject;
        [SerializeField] private TextMeshProUGUI _locationText;

        [SerializeField] private InGameButtonHolder[] _standInfoButtons;
        
        public static InGamePanelController Instance;

        protected override void AfterAwake()
        {
            HideStandInfoButtons();
            Instance = this;
        }

        protected override void AfterStart()
        {
            UpdateLocationText();
        }

        private void OnEnable()
        {
            UpdateLocationText();
        }

        private void UpdateLocationText()
        {
            _locationText.text = "";

            if (ConferenceRoomManager.Instance != null)
            {
                string sceneName = ConferenceRoomManager.Instance.CurrentRoom.SceneName;
                int floorId = (int)ConferenceRoomManager.Instance.CurrentRoom.FloorId;

                Tuple<string, int> currentLocation = new Tuple<string, int>(sceneName, floorId);

                List<Tuple<string, int, string>> halls = new List<Tuple<string, int, string>>();

                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.DigitalAutomation),      0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/DigitalAutomationButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Energy),                 0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/EnergyButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.ExtractionProcessing),   0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/ExtractionProcessingButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Government),             0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/GovernmentButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.HSSE),                   0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/HSSEButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Partners),               0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/PartnersButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Telecommunication),      0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/TelecommunicationButton")));
                halls.Add(new Tuple<string, int, string>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.TransportationStorage),  0, I2.Loc.LocalizationManager.GetTranslation("Map/TopButtons/TransportationStorageButton")));
                
                var location = halls.Find(x => (x.Item1 == currentLocation.Item1 && x.Item2 == currentLocation.Item2));
                if (location != null && !string.IsNullOrEmpty(location.Item3))
                {
                    _locationText.text = location.Item3;
                }
            }
        }

        public void ShowStandInfoButtons(int companyLinksCount)
        {
            if (companyLinksCount > 0)
            {
                InGameButtonHolder media = _standInfoButtons.FirstOrDefault(a => a.Type == InGameButtonType.Media);
                media?.Enable();
            }

            InGameButtonHolder contacts = _standInfoButtons.FirstOrDefault(a => a.Type == InGameButtonType.Contacts);
            contacts?.Enable();
        }

        public void HideStandInfoButtons()
        {
            foreach (InGameButtonHolder inGameButtonHolder in _standInfoButtons)
            {
                inGameButtonHolder.Disable();
            }
        }

        public void OnMainMenuClick()
        {
            SwitchTo(ExpoUIType.MainMenu);
        }

        public void OnContactsClick()
        {
            SwitchTo(ExpoUIType.CompanyInfo);
        }

        public void SetChatCount(int count)
        {
            _newMessagesObject.SetUnreadCount(count);
        }

        public void OnStandInfoClick()
        {
            SwitchTo(ExpoUIType.StandInfo);
        }

        public void OnChatClick()
        {
            ExpoChatController.Instance.Show();
            _chatButton.SetActive(false);
        }

        public void ShowChat()
        {
            _chatButton.SetActive(true);
        }


        public void OnHelpClick()
        {
            SwitchTo(ExpoUIType.Help);
        }

        [Serializable]
        private class InGameButtonHolder
        {
            [SerializeField] private GameObject _button;
            [SerializeField] private InGameButtonType _type;

            public InGameButtonType Type => _type;

            public void Enable()
            {
                _button.SetActive(true);
            }

            public void Disable()
            {
                _button.SetActive(false);
            }
        }

        private enum InGameButtonType
        {
            Contacts,
            Media
        }
    }
}