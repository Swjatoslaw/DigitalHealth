using _IExpo.Scripts.ExpoStand.Interaction.FocusController;
using _IExpo.Scripts.ExpoStand.ShowPieces;
using UnityEngine;

namespace _IExpo.Scripts.ExpoStand.Interaction
{
    public class ExpoModelInteractor : BaseStandShowPieceInteractor<ModelFocusController>
    {
        [SerializeField] private ExpoStandModel _standModel;

        private ModelFocusController _modelFocusController;

        protected override ModelFocusController FocusController => _modelFocusController;

        protected override void InitialiseStrategy()
        {
            _modelFocusController = new ModelFocusController(_standModel);
        }
    }
}