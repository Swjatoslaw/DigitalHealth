using _IExpo.Scripts.ExpoStand.Interaction.FocusController;
using UnityEngine;

namespace _IExpo.Scripts.ExpoStand.Interaction
{
    public abstract class BaseStandShowPieceInteractor<T> : MonoBehaviour, IStandInteractor
        where T : IFocusController
    {
        [SerializeField] private Renderer _renderer;

        protected abstract T FocusController { get; }

        public virtual void Highlight()
        {
            _renderer.materials[0].SetColor("_BaseColor", Color.yellow);
        }

        public virtual void RemoveHighlight()
        {
            _renderer.materials[0].SetColor("_BaseColor", Color.white);
        }

        private void Awake()
        {
            InitialiseStrategy();
        }

        public void Focus()
        {
            if (FocusController != null) FocusController.Focus();
            else Debug.LogError("Focus controller is not initalised");
        }

        public void UnFocus()
        {
            if (FocusController != null) FocusController.UnFocus();
            else Debug.LogError("Focus controller is not initalised");
        }

        protected abstract void InitialiseStrategy();
    }
}