using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoValidation
{
    public class ValidationManager : MonoBehaviour
    {
        [SerializeField] private List<ValidationBase> _inputFields;
        [SerializeField] private UnityEvent _ifValid;
        private int _actionsCount = 0;
        private bool _isValid = true;
        public void CheckValidation()
        {
            _isValid = true;
            _actionsCount = 0;
            foreach (ValidationBase inputField in _inputFields)
            {
                inputField.HideAllWarnings();
            }
            foreach (ValidationBase inputField in _inputFields)
            {
                inputField.IsFieldValid(DoIfValid, DoIfNotValid);
            }

        }
        public void CheckValidationByIndex(int index, UnityAction ifValid, UnityAction ifNotValid)
        {
            _inputFields[index].HideAllWarnings();
            _inputFields[index].IsFieldValid(ifValid, ifNotValid);
        }
        private void DoIfValid()
        {
            _actionsCount++;
            if (_actionsCount == _inputFields.Count && _isValid)
            {
                _ifValid.Invoke();
            }
        }
        private void DoIfNotValid()
        {
            _actionsCount++;
            _isValid = false;
        }
    }
}
