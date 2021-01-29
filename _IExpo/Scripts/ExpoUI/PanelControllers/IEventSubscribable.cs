using System;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public interface IEventSubscribable<T>
        where T : Enum
    {
        void Subscribe(Action<T> action);
        void Unsubscribe(Action<T> action);
    }
}