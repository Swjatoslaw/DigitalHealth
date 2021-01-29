namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class ChatComponentsPanel : BasePanelController
    {
        public void OnBackClick()
        {
            SwitchTo(ExpoUIType.InGame);
        }
    }
}