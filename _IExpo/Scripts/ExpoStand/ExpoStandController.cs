using System;
using System.Collections.Generic;
using System.Linq;
using _IExpo.Scripts.ExpoSO;
using _IExpo.Scripts.ExpoUI.PanelControllers;
using _IExpo.Scripts.ExpoUtils;
using JetBrains.Annotations;
using SpaceHub.Conference;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _IExpo.Scripts.ExpoStand
{
    public class ExpoStandController : MonoBehaviour
    {
        [ItemCanBeNull] private List<ExpoStandCompanyInfoSO> _companies;

        [SerializeField] private AssetReference _companyReference;

        [SerializeField] private Renderer _logoRenderer;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private SpawnPoint _spawnPoint;

        [SerializeField] private List<FacesHolder> _facesHolders;
        public List<FacesHolder> FacesHolders => _facesHolders;

        public SpawnPoint SpawnPoint => _spawnPoint;

        private ConferenceRoomManager.Room _currentRoom;
        private ExpoStandCompanyInfoSO _companyInfo;
        private AsyncOperationHandle<ExpoStandCompanyInfoSO> _handle;


        private void Awake()
        {
            if (_triggerCollider != null)
            {
                _triggerCollider.enabled = false;
            }
        }

        private void Start()
        {
            WaitForPhoton waitForPhoton = new WaitForPhoton();

            StartCoroutine(waitForPhoton.Start(AfterConnectedToPhoton));
        }


        private void AfterConnectedToPhoton()
        {
            _handle = _companyReference.LoadAssetAsync<ExpoStandCompanyInfoSO>();

            _handle.Completed += operationHandle =>
            {
                if (!operationHandle.IsValid())
                {
                    Debug.LogError("Could not load company Addressable");
                    return;
                }

                _currentRoom = ConferenceRoomManager.Instance.CurrentRoom;

                _companyInfo = operationHandle.Result;

                if (_companyInfo == null)
                {
                    gameObject.SetActive(false);
                    return;
                }

                LoadStandData(_companyInfo);
            };
        }

        private void OnDestroy()
        {
            if (_handle.IsValid() && _handle.IsDone)
            {
                Addressables.Release(_handle);
            }
        }

        public void LoadStandData(ExpoStandCompanyInfoSO companyInfo)
        {
            _companyInfo = companyInfo;

            if (_triggerCollider != null)
            {
                _triggerCollider.enabled = true;
            }

            foreach (ExpoStandData expoStandData in _companyInfo.StandDatas)
            {
                var faceInfo = _facesHolders.FirstOrDefault(a => a.FaceId == expoStandData.FaceId);

                if (faceInfo == null)
                {
                    Debug.LogError($"Could not find face id: {expoStandData.FaceId}");
                    continue;
                }

                faceInfo.Television.SetCompanyId(_companyInfo.CompanyId);
                faceInfo.Television.SetFace(faceInfo.FaceType, expoStandData);
            }

            foreach (FacesHolder facesHolder in _facesHolders)
            {
                if (_companyInfo.CompanyLogo != null)
                    facesHolder.Television.SetPlaceHolder(_companyInfo.CompanyLogo.texture);
            }

            if (_logoRenderer != null)
            {
                _logoRenderer.materials[1].SetTexture("_BaseMap", _companyInfo.CompanyLogo.texture);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LocalPlayer") && _companyInfo != null)
            {
                InGamePanelController.Instance?.ShowStandInfoButtons(_companyInfo.CompanyLinks.Count);

                if (_currentRoom != null)
                {
                    CompanyInfoPanelController.Instance?.Fill(_companyInfo);
                    StandInfoControllerPanel.Instance?.Fill(_companyInfo);
                }
            }
            else if (_companyInfo == null && other.CompareTag("LocalPlayer"))
            {
                this.DLogError($"{gameObject.name} Has floor stand info as null");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LocalPlayer"))
            {
                InGamePanelController.Instance.HideStandInfoButtons();
            }
        }

#if UNITY_EDITOR
        public void AddFace()
        {
            if (_facesHolders == null)
            {
                _facesHolders = new List<FacesHolder>();
            }

            _facesHolders.Add(new FacesHolder());
        }

        public async void AutoSetFaceId()
        {
            if (_spawnPoint == null)
            {
                Debug.LogError("Could not set spawn point because it is not set");
                return;
            }

            var company = ((ExpoStandCompanyInfoSO) _companyReference.editorAsset);
            Debug.Log($"Setting up: {company.CompanyId}");
            _spawnPoint.Id = $"Stand_{company.CompanyId}";

            EditorUtility.SetDirty(_spawnPoint);
        }

#endif
    }

    [Serializable]
    public class FacesHolder
    {
        [SerializeField] private int _faceId;
        [SerializeField] private FaceType _faceType;
        [SerializeField] private TeleController _television;

        public int FaceId => _faceId;

        public TeleController Television => _television;

        public FaceType FaceType => _faceType;
    }
}