using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoValidation
{
    public class EqualPasswordsValidation : ValidationBase
    {
        [SerializeField] private TMP_InputField _firstPassword;
        public override void IsFieldValid(UnityAction callbackIfValid, UnityAction callbackIfNotValid)
        {
            if (ThisField.text.Length >= MinLength && ThisField.text == _firstPassword.text)
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
            if (ThisField.text.Length < MinLength)
            {
                ShowWarningOfType(WarningsEnum.MinLength);
                return;
            }

            else if (ThisField.text != _firstPassword.text)
            {
                ShowWarningOfType(WarningsEnum.NotEqualPasswords);
                return;
            }
        }
    }
}