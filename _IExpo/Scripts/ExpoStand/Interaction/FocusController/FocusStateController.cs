using UnityEngine;
using UnityEngine.EventSystems;

namespace _IExpo.Scripts.ExpoStand.Interaction.FocusController
{
    public class FocusStateController
    {
        private float _unfocusTime;

        private const float UnfocusDelay = 0.3f;

        private ShowPieceInteractorState CurrentState { get; set; }
        private bool IsFocused => CurrentState == ShowPieceInteractorState.Focused;

        public bool IsBusy =>
            IsFocused || EventSystem.current.currentSelectedGameObject != null ||
            EventSystem.current.IsPointerOverGameObject() || _unfocusTime > 0.01f;

        public void HandleFocus()
        {
            CurrentState = ShowPieceInteractorState.Focused;
        }

        public void HandleUnfocus()
        {
            CurrentState = ShowPieceInteractorState.Empty;
            _unfocusTime = UnfocusDelay;
        }

        public void UpdateState()
        {
            _unfocusTime -= Time.deltaTime;
        }
    }
}