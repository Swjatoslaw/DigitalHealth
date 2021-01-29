using System.Collections;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    public class ViewOverlay : MonoBehaviour
    {
        [SerializeField] private GameObject _overlay;

        public static ViewOverlay Instance;

        private void Awake()
        {
            Instance = this;
            _overlay.SetActive(false);
        }

        public void EnableFor(float time)
        {
            StartCoroutine(EnableForCoroutine(time));
        }

        private IEnumerator EnableForCoroutine(float time)
        {
            _overlay.SetActive(true);

            yield return new WaitForSeconds(time);

            _overlay.SetActive(false);
        }
    }
}