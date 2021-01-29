using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public abstract class BasePanelController : MonoBehaviour
    {
        [SerializeField] private ExpoUIManager _uiManager;

        private void Awake()
        {
            _uiManager = GetComponentInParent<ExpoUIManager>();
            AfterAwake();
        }

        private void Start()
        {
            AfterStart();
        }

        public void SwitchTo(ExpoUIType uiType)
        {
            _uiManager?.ShowUI(uiType);
        }

        protected virtual void AfterAwake()
        {
        }

        protected virtual void AfterStart()
        {
        }
    }
}