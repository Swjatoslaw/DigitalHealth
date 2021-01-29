using System;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class StandSlideShowPanelController : BasePanelController, IEventSubscribable<StandSlidePanelEventType>
    {
        private event Action<StandSlidePanelEventType> _controlEvent;

        public static StandSlideShowPanelController Instance;

        protected override void AfterAwake()
        {
            Instance = this;
        }

        public void Subscribe(Action<StandSlidePanelEventType> action)
        {
            _controlEvent += action;
        }

        public void Unsubscribe(Action<StandSlidePanelEventType> action)
        {
            _controlEvent -= action;
        }

        public void OnNextClick()
        {
            _controlEvent?.Invoke(StandSlidePanelEventType.Next);
        }

        public void OnCloseClick()
        {
            _controlEvent?.Invoke(StandSlidePanelEventType.Close);
            SwitchTo(ExpoUIType.InGame);
        }

        public void OnPrevClick()
        {
            _controlEvent?.Invoke(StandSlidePanelEventType.Prev);
        }
    }

    public enum StandSlidePanelEventType
    {
        Prev,
        Next,
        Close
    }
}