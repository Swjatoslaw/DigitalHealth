using _IExpo.Scripts.ExpoCharacter;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers.MainMenuPanel
{
    public class MainMenuPanel : BaseTabPanel<MainMenuTabHolder, MainMenuTabType>
    {
        public GameObject appExitButton;
        
        private void Start()
        {
            SwitchTab(MainMenuTabType.Map);
        }

        public void OnSettingsClick()
        {
            SwitchTab(MainMenuTabType.Settings);
        }

        public void OnMapClick()
        {
            SwitchTab(MainMenuTabType.Map);
        }

        public void OnTimeTableClick()
        {
            SwitchTab(MainMenuTabType.TimeTable);
        }

        public void OnBusinessCardClick()
        {
            SwitchTab(MainMenuTabType.BusinessCard);
        }

        public void OnContactListClick()
        {
            SwitchTab(MainMenuTabType.ContactList);
        }

        public void OnCloseClick()
        {
            SwitchTo(ExpoUIType.InGame);
        }

        public void OnAppExitClick()
        {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        protected override void AfterStart()
        {
            base.AfterStart();

            var expoRig = FindObjectOfType<ExpoRig>();

            if (expoRig != null && appExitButton != null)
            {
                appExitButton.SetActive(!expoRig.IsMobileInterface);
            }
        }
    }
}