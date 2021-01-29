namespace _IExpo.Scripts.ExpoStand.Interaction.FocusController
{
    public abstract class BaseFocusController : IFocusController
    {
        private readonly FocusStateController _focusState = new FocusStateController();

        public void Focus()
        {
            if (_focusState.IsBusy)
            {
                return;
            }

            _focusState.HandleFocus();
            AfterFocusStateCheck();
        }

        public void UpdateUnfocusTime()
        {
            _focusState.UpdateState();
        }

        protected abstract void AfterFocusStateCheck();

        public void UnFocus()
        {
            _focusState.HandleUnfocus();
            AfterUnFocusStateCheck();
        }

        protected abstract void AfterUnFocusStateCheck();
    }
}