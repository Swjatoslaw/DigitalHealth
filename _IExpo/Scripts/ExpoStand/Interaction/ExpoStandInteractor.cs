using _IExpo.Scripts.ExpoStand.ShowPieces;
using UnityEngine;

namespace _IExpo.Scripts.ExpoStand.Interaction
{
    public class ExpoStandInteractor : BaseStandShowPieceInteractor<TeleStandFocusController>
    {
        [SerializeField] private BaseStandShowPiece _stand;

        private TeleStandFocusController _standFocusController;
        protected override TeleStandFocusController FocusController => _standFocusController;

        protected override void InitialiseStrategy()
        {
            _stand = GetComponentInParent<BaseStandShowPiece>();

            RemoveHighlight();

            _standFocusController = new TeleStandFocusController(_stand);
        }

        private void Update()
        {
            _standFocusController.UpdateUnfocusTime();
        }
    }
}