using System;
using _IExpo.Scripts.ExpoStand.ShowPieces;
using UnityEngine;

namespace _IExpo.Scripts.ExpoStand
{
    [Serializable]
    public class StandHolder
    {
        [SerializeField] private GameObject _stand;
        [SerializeField] private StandType _standType;

        public StandType StandType => _standType;

        public GameObject Stand => _stand;

        public BaseStandShowPiece GetStand()
        {
            return _stand.GetComponent<BaseStandShowPiece>();
        }

        public void Show()
        {
            BaseStandShowPiece baseStand = _stand.GetComponent<BaseStandShowPiece>();
            baseStand.Show();
            baseStand.Subscribe();
        }

        public void Enable()
        {
            _stand.SetActive(true);
        }

        public void Disable()
        {
            _stand.SetActive(false);
        }

        public void Hide()
        {
            var baseStand = _stand.GetComponent<BaseStandShowPiece>();
            baseStand.Hide();
            baseStand.Unsubscribe();
        }
    }
}