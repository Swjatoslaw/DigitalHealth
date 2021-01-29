using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoValidation
{
    public abstract class ValidationBase : MonoBehaviour
    {
        [Range(1, 254)]
        [SerializeField] private int _minLength;
        [SerializeField] private TMP_InputField _thisField;
        [SerializeField] private List<WarningBase> _warningsList;
        protected int MinLength { get => _minLength; }
        protected TMP_InputField ThisField { get => _thisField; }

        public abstract void IsFieldValid(UnityAction callbackIfValid, UnityAction callbackIfNotValid);
        public void HideAllWarnings()
        {
            foreach (WarningBase warning in _warningsList)
            {
                warning.WarningObject.SetActive(false);
            }
        }
        public abstract void ShowWarning();
        protected void ShowWarningOfType(WarningsEnum type)
        {
            foreach(WarningBase warning in _warningsList)
            {
                if (warning.WarningType == type)
                {
                    warning.WarningObject.SetActive(true);
                    break;
                }
            }
        }
    }
}