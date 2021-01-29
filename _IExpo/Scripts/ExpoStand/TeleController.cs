using System;
using System.Linq;
using _IExpo.Scripts.ExpoBackend;
using _IExpo.Scripts.ExpoSO;
using _IExpo.Scripts.ExpoStand.ShowPieces;
using _IExpo.Scripts.ExpoUI.PanelControllers;
using UnityEngine;

namespace _IExpo.Scripts.ExpoStand
{
    public class TeleController : MonoBehaviour
    {
        private int _companyId;
        
        [Tooltip("Material index 2 is face 1, index 1 is face 2")] [SerializeField]
        private FaceType _showFaceType;

        [SerializeField] private FacedStandHolder[] _standHolders;

        private CurrentFaceHolder _currentFace1 = new CurrentFaceHolder();
        private CurrentFaceHolder _currentFace2 = new CurrentFaceHolder();

        public FaceType ShowFaceType => _showFaceType;

        public FacedStandHolder[] StandHolders => _standHolders;

        private void Start()
        {
            SetFace(FaceType.Face1, null);
            SetFace(FaceType.Face2, null);
        }

        public void SetCompanyId(int id)
        {
            _companyId = id;
        }

        public void SetPlaceHolder(Texture texture)
        {
            var firstFace = _standHolders.FirstOrDefault(a =>
                a.FaceType1 == FaceType.Face1 && _currentFace1.StandType == a.StandType);

            if (firstFace != null)
            {
                firstFace.Stand.SetPlaceHolder(texture);
            }


            var secondFace = _standHolders.FirstOrDefault(a =>
                a.FaceType1 == FaceType.Face2 && _currentFace2.StandType == a.StandType);

            if (secondFace != null)
            {
                secondFace.Stand.SetPlaceHolder(texture);
            }
        }

        public void FireInteractionEvent()
        {
            CompanyInfoLeaver companyInfoLeaver = new CompanyInfoLeaver(this);
            companyInfoLeaver.GenerateLeadSignal(_companyId, 2, ExpoLeadType.Cold);
        }

        public void SetFace(FaceType faceType, ExpoStandData standData)
        {
            if (standData == null)
            {
                var emptyHolder =
                    _standHolders.FirstOrDefault(a => a.FaceType1 == faceType && a.StandType == StandType.Empty);
                emptyHolder.Stand.gameObject.SetActive(true);

                if (faceType == FaceType.Face1)
                {
                    _currentFace1.FaceType = FaceType.Face1;
                    _currentFace1.StandType = StandType.Empty;
                }
                else
                {
                    _currentFace2.FaceType = FaceType.Face2;
                    _currentFace2.StandType = StandType.Empty;
                }

                emptyHolder?.Stand.SetData(null);
                emptyHolder?.Stand.SetPlaceHolder(null);
                return;
            }

            var standHolder =
                _standHolders.FirstOrDefault(a => a.FaceType1 == faceType && a.StandType == standData.StandType);

            if (standHolder == null)
            {
                Debug.LogError("Could not find suitable candidate");
                return;
            }

            var holders = _standHolders.Where(a => a.FaceType1 == faceType);

            foreach (FacedStandHolder facedStandHolder in holders)
            {
                facedStandHolder?.Stand.gameObject.SetActive(false);
            }

            standHolder.Stand.gameObject.SetActive(true);
            standHolder.Stand.SetData(standData);

            if (faceType == FaceType.Face1)
            {
                _currentFace1.FaceType = FaceType.Face1;
                _currentFace1.StandType = standHolder.StandType;
            }
            else
            {
                _currentFace2.FaceType = FaceType.Face2;
                _currentFace2.StandType = standHolder.StandType;
            }
        }

        private class CurrentFaceHolder
        {
            public FaceType FaceType;
            public StandType StandType;
        }
    }

    [Serializable]
    public class FacedStandHolder
    {
        [SerializeField] private FaceType _faceType;
        [SerializeField] private BaseStandShowPiece _stand;
        [SerializeField] private StandType _standType;
        public FaceType FaceType1 => _faceType;

        public BaseStandShowPiece Stand => _stand;

        public StandType StandType => _standType;
    }

    public enum FaceType
    {
        Face1,
        Face2
    }
}