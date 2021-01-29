using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoValidation
{
    public class MinLengthValidation : ValidationBase
    {
        public override void IsFieldValid(UnityAction callbackIfValid, UnityAction callbackIfNotValid)
        {
            if (ThisField.text.Length >= MinLength)
            {
                callbackIfValid();
            }
            else
            {
                ShowWarning();
                callbackIfNotValid();
            }
        }

        public override void ShowWarning()
        {
            ShowWarningOfType(WarningsEnum.MinLength);
        }
    }
}
