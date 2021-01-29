using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.Avatar
{
    public class AvatarBadgeDisabler : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<AvatarBadgeDisablerTrigger>().Ab.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<AvatarBadgeDisablerTrigger>().Ab.gameObject.SetActive(false);
        }

    }
}
