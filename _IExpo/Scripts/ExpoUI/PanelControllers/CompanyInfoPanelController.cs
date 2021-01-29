using _IExpo.Scripts.ExpoBackend;
using _IExpo.Scripts.ExpoSO;
using SpaceHub.Conference;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class CompanyInfoPanelController : BasePanelController
    {
        [SerializeField] private TextMeshProUGUI _companyName;
        [SerializeField] private Image _companyLogo;
        [SerializeField] private GameObject _companyLinkPrefab;
        [SerializeField] private Transform _companyCountryInfoParent;
        [SerializeField] private GameObject _visitSiteButton;
        [SerializeField] private GameObject _leaveContactButton;

        public static CompanyInfoPanelController Instance;

        private ExpoStandCompanyInfoSO _currentCompanyInfo;
        private Dictionary<int, bool> _interactedCompanies = new Dictionary<int, bool>();

        private ExpoCompanyLeaveInfo _companyInfo = new ExpoCompanyLeaveInfo();

        protected override void AfterAwake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
        }

        public void Fill(ExpoStandCompanyInfoSO companyInfoSo)
        {
            _currentCompanyInfo = companyInfoSo;

            _companyName.text = companyInfoSo.CompanyName;
            _companyLogo.sprite = companyInfoSo.CompanyLogo;

            _visitSiteButton.SetActive(!string.IsNullOrEmpty(companyInfoSo.CompanyUrl));

            for (int i = 0; i < _companyCountryInfoParent.childCount; i++)
            {
                var child = _companyCountryInfoParent.GetChild(i);
                Destroy(child.gameObject);
            }

            foreach (var countryInfo in companyInfoSo.CompanyCountryInfos)
            {
                var country = Instantiate(_companyLinkPrefab, _companyCountryInfoParent);
                country.GetComponent<CompanyInfoLinkController>()?.SetData(countryInfo);
            }

            if (!_interactedCompanies.ContainsKey(companyInfoSo.CompanyId))
            {
                _interactedCompanies.Add(companyInfoSo.CompanyId, false);
            }
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(SpaceHubLogin.Token))
            {
                _leaveContactButton?.SetActive(false);
            }
            else
            {
                _leaveContactButton?.SetActive(true);

                if (_currentCompanyInfo != null)
                {
                    CompanyInfoLeaver companyInfoLeaver = new CompanyInfoLeaver(this);
                    companyInfoLeaver.GenerateLeadSignal(_currentCompanyInfo.CompanyId, 2, ExpoLeadType.Cold);
                }
            }
        }

        public void OnCloseClick()
        {
            SwitchTo(ExpoUIType.InGame);
        }

        public void OnOpenWebsiteClick()
        {
            Application.OpenURL(_currentCompanyInfo.CompanyUrl);
        }

        public void OnLeaveContactClick()
        {
            print($"Leave contact click: {string.IsNullOrEmpty(SpaceHubLogin.Token)}");
            if (_interactedCompanies.ContainsKey(_currentCompanyInfo.CompanyId) &&
                !_interactedCompanies[_currentCompanyInfo.CompanyId] && !string.IsNullOrEmpty(SpaceHubLogin.Token))
            {
                CompanyInfoLeaver companyInfoLeaver = new CompanyInfoLeaver(this);
                companyInfoLeaver.GenerateLeadSignal(_currentCompanyInfo.CompanyId, 2, ExpoLeadType.Hot, false);
            }
        }



    }
}