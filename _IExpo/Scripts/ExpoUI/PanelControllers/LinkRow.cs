using _IExpo.Scripts.ExpoSO;
using SpaceHub.Conference;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class LinkRow : MonoBehaviour
    {
        private CompanyLink _companyLink;

        public void OnSelfClick()
        {
            Application.OpenURL(_companyLink.LinkUrl);
        }

        public void Fill(CompanyLink companyLink)
        {
            _companyLink = companyLink;
        }
    }
}