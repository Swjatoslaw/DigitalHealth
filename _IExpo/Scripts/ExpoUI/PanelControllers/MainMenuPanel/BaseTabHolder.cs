using System;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers.MainMenuPanel
{
    [Serializable]
    public abstract class BaseTabHolder<T> : IControllable
        where T : Enum
    {
        [SerializeField] private GameObject _tabBtn;
        public GameObject TabBtn => _tabBtn;
        
        [SerializeField] private GameObject _tabObject;
        public GameObject TabObject => _tabObject;

        [SerializeField] private T _tab;
        public T Tab => _tab;

        public void Activate()
        {
            if (_tabObject != null)
            {
                _tabObject.SetActive(true);
            }

            if (_tabBtn != null)
            {
                var controllable = _tabBtn.GetComponent<IControllable>();
                controllable?.Activate();
            }
        }

        public void Deactivate()
        {
            if (_tabObject != null)
            {
                _tabObject.SetActive(false);
            }

            if (_tabBtn != null)
            {
                var controllable = _tabBtn.GetComponent<IControllable>();
                controllable?.Deactivate();
            }
        }
    }
}