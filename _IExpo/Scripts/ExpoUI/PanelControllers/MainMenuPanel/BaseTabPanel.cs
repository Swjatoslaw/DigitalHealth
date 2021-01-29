using System;
using System.Linq;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers.MainMenuPanel
{
    public abstract class BaseTabPanel<T, V> : BasePanelController
        where T : BaseTabHolder<V>
        where V : Enum
    {
        [SerializeField] private T[] _tabs;
        public T[] Tabs => _tabs; 
        
        [SerializeField] private T _currentTab;
        public T CurrentTab => _currentTab; 

        protected virtual void SwitchTab(V tabType)
        {
            _currentTab = _tabs.FirstOrDefault(a => Equals(a.Tab, tabType));

            if (_currentTab != null)
            {
                foreach (T menuTab in _tabs)
                {
                    menuTab.Deactivate();
                }

                _currentTab.Activate();
            }
        }
    }
}