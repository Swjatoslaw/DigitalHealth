using _IExpo.Scripts.ExpoSO;
using TMPro;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class CompanyInfoLinkController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countryName;
        [SerializeField] private TextMeshProUGUI _email;
        [SerializeField] private TextMeshProUGUI _contactPhoneNumber;

        public void SetData(CompanyCountryInfo countryInfo)
        {
            _countryName.text = countryInfo.CompanyCountry;
            _email.text = countryInfo.CompanyEmail;
            _contactPhoneNumber.text = countryInfo.CompanyContactPhone;
        }
    }
}