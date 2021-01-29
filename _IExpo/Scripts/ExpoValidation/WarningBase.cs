using UnityEngine;

namespace _IExpo.Scripts.ExpoValidation
{
    [System.Serializable]
    public struct WarningBase
    {
        [SerializeField] private GameObject _warningObject;
        [SerializeField] private WarningsEnum _warningType;

        public WarningsEnum WarningType { get => _warningType;}
        public GameObject WarningObject { get => _warningObject; }
    }
}
