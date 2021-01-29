using System;
using System.Linq;
using _IExpo.Scripts.ExpoSO;
using UnityEngine;
using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class StandInfoControllerPanel : BasePanelController
    {
        [SerializeField] private LinkHolder[] _linkHolders;
        [SerializeField] private Transform _linkHoldersParent;

        public static StandInfoControllerPanel Instance;

        protected override void AfterAwake()
        {
            Instance = this;
        }

        public void Fill(ExpoStandCompanyInfoSO companyInfoSo)
        {
            for (int i = 0; i < _linkHoldersParent.childCount; i++)
            {
                Transform child = _linkHoldersParent.GetChild(i);
                Destroy(child.gameObject);
            }

            foreach (CompanyLink link in companyInfoSo.CompanyLinks)
            {
                LinkHolder linkHolder = _linkHolders.FirstOrDefault(a => a.Type == link.Type);

                if (linkHolder != null)
                {
                    var obj = Instantiate(linkHolder.Prefab, _linkHoldersParent);
                    obj.GetComponent<LinkRow>().Fill(link);
                }
            }
        }

        public void OnViewVideoClick()
        {
            print("ParentClickVideo");
            SwitchTo(ExpoUIType.StandVideo);
        }

        public void OnViewPresentationClick()
        {
            SwitchTo(ExpoUIType.StandSlideShow);
        }

        public void OnCloseClick()
        {
            SwitchTo(ExpoUIType.InGame);
        }

        public void OnViewCompanyInfoClick()
        {
            SwitchTo(ExpoUIType.CompanyInfo);
        }
    }


    [Serializable]
    public class LinkHolder
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private CompanyLinkType _type;

        public CompanyLinkType Type => _type;
        public GameObject Prefab => _prefab;
    }
}