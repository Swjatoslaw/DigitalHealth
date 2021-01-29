using _IExpo.Scripts.ExpoStand.ShowPieces;

namespace _IExpo.Scripts.ExpoStand.Interaction.FocusController
{
    public class ModelFocusController : BaseFocusController
    {
        private ExpoStandModel _standModel;

        public ModelFocusController(ExpoStandModel standModel)
        {
            _standModel = standModel;
        }

        protected override void AfterFocusStateCheck()
        {
            _standModel.Show();
        }

        protected override void AfterUnFocusStateCheck()
        {
            _standModel.Hide();
        }
    }
}