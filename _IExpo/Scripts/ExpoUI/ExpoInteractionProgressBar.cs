using Michsky.UI.ModernUIPack;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using SpaceHub.Conference;
using UnityEngine;
using UnityEngine.UI;

namespace _IExpo.Scripts.ExpoUI
{
    [RequireComponent(typeof(ProgressBar))]
    public class ExpoInteractionProgressBar : MonoBehaviour
    {
        public ProgressBar progressBar;

        public ExpoInteractionsImagesLibraryScriptableObject interactionsLibrary;
        public CursorType interactionType;

        public Image interactionTypeImage;

        private void Awake()
        {
            if (progressBar == null)
            {
                progressBar = GetComponent<ProgressBar>();
            }

            if (interactionTypeImage == null)
            {
                interactionTypeImage = transform.Find("InteractionTypeImage").GetComponent<Image>();
            }

            SetInteractionType(interactionType);
        }

        public void SetInteractionType(CursorType newType)
        {
            interactionType = newType;

            interactionTypeImage.sprite = interactionsLibrary.GetSpriteByType(interactionType);
        }

        public void SetCurrentPercent(float currentPercent)
        {
            progressBar.currentPercent = Mathf.Clamp(currentPercent, 0f, 100f);
        }
    }
}