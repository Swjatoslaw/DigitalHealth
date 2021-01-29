using _IExpo.Scripts.ExpoStand.Interaction.FocusController;
using _IExpo.Scripts.ExpoStand.ShowPieces;

namespace _IExpo.Scripts.ExpoStand.Interaction
{
    public class TeleStandFocusController : BaseFocusController
    {
        private readonly BaseStandShowPiece _stand;

        public TeleStandFocusController(BaseStandShowPiece stand)
        {
            _stand = stand;
        }

        protected override void AfterFocusStateCheck()
        {
            _stand.Subscribe();
            _stand.Show();
        }

        protected override void AfterUnFocusStateCheck()
        {
            _stand.Unsubscribe();
        }
    }

    public enum ShowPieceInteractorState
    {
        Empty,
        Focused,
    }
}