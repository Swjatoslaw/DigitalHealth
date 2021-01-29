using System;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class HelpPanelController : BasePanelController, IEventSubscribable<StandSlidePanelEventType>
    {
        [SerializeField] private ExpoUISlideShowPresentation _expoUISlideShowPresentation;

        private event Action<StandSlidePanelEventType> _controlEvent;

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
    
}