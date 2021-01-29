using _IExpo.Scripts.ExpoUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace _IExpo.Scripts.ExpoUI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ExpoUICopyToClipboard : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _textComponent;

        private void Awake()
        {
            if(_textComponent == null)
            {
                _textComponent = GetComponent<TextMeshProUGUI>();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartCoroutine(CopyToClipboardRoutine());
        }

        private IEnumerator CopyToClipboardRoutine()
        {
            if(_textComponent != null)
            {
                ExpoClipboardExtension.CopyToClipboard(_textComponent.text);

                ExpoCopyToClipboardScreen.Instance.Show();
            }

            yield return null;
        }

        
    }
}