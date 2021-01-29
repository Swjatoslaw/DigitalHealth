using System;
using System.Collections;
using _IExpo.Scripts.ExpoSO;
using _IExpo.Scripts.ExpoStand;
using _IExpo.Scripts.ExpoUI;
using _IExpo.Scripts.ExpoUI.PanelControllers;
using _IExpo.Scripts.ExpoUtils;
using I2.Loc;
using Paroxe.PdfRenderer;
using Paroxe.PdfRenderer.WebGL;
using SpaceHub.Conference;
using UnityEngine;
using UnityEngine.UI;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoUISlideShowPresentation : MonoBehaviour
    {
        [SerializeField] private HelpPanelController _helpPanelController;
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private AspectRatioFitter _aspectRatioFitter;

        [SerializeField] private string _manualURL_LocalizationTerm = "GamePanel/Support/ManualPDF";
        [SerializeField] private string _manualURL_LocalizationTerm_Mobile = "GamePanel/Support/ManualPDFMobile";

        private int _currentPage = 0;

        private PDFDocument _document;
        private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");

        private string _currentLanguage;

        private void Awake()
        {
            if (_helpPanelController == null)
            {
                _helpPanelController = GetComponentInParent<HelpPanelController>();
            }

            if (_rawImage == null)
            {
                _rawImage = GetComponent<RawImage>();
            }

            if (_aspectRatioFitter == null)
            {
                _aspectRatioFitter = GetComponent<AspectRatioFitter>();
            }

            _currentLanguage = "";
        }

        private void Start()
        {
            Subscribe();
        }

        private void OnEnable()
        {
            Load();
        }

        private void Subscribe()
        {
            _helpPanelController.Subscribe(UIControlClick);
        }

        private void UIControlClick(StandSlidePanelEventType eventType)
        {
            switch (eventType)
            {
                case StandSlidePanelEventType.Prev:
                    LoadPrev();
                    break;
                case StandSlidePanelEventType.Next:
                    LoadNext();
                    break;
                case StandSlidePanelEventType.Close:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }

        public void Load()
        {
            if (_document == null || _currentLanguage != LocalizationManager.CurrentLanguage)
            {

                ExpoLoadingScreen.Instance?.Show();

                _currentLanguage = LocalizationManager.CurrentLanguage;
                _document = null;
                
                string link = LocalizationManager.GetTranslation(_manualURL_LocalizationTerm);

                if (ViewModeManager.Instance.GetSelectedViewMode() == ViewModeManager.ViewMode.Mobile)
                {
                    if (!string.IsNullOrEmpty(LocalizationManager.GetTranslation(_manualURL_LocalizationTerm_Mobile)))
                    {
                        link = LocalizationManager.GetTranslation(_manualURL_LocalizationTerm_Mobile);
                    }
                }

                PDFJS_Promise<PDFDocument> promise = PDFDocument.LoadDocumentFromUrlAsync(link);

                StartCoroutine(WaitForLoad(promise));
            }
        }

        private IEnumerator WaitForLoad(PDFJS_Promise<PDFDocument> promise)
        {
            float passedTime = 0f;
            while (!promise.HasFinished)
            {
                passedTime += Time.deltaTime;

                if (passedTime > 13f)
                {
                    yield break;
                }

                yield return null;
            }

            if (promise.HasSucceeded)
            {
                _document = promise.Result;

                if (_document.IsValid)
                {
                    _currentPage = 0;
                    SetPage(_currentPage);
                }
            }
            else
            {
                this.DLogError(
                    $"Could not load presentation:{LocalizationManager.GetTranslation(_manualURL_LocalizationTerm)}");
            }

            ExpoLoadingScreen.Instance?.Hide();

        }

        private void LoadNext()
        {
            _currentPage = (_currentPage + 1) % _document.GetPageCount();

            SetPage(_currentPage);
        }

        private void LoadPrev()
        {
            _currentPage = Mathf.Max(_currentPage - 1, 0) % _document.GetPageCount();
            SetPage(_currentPage);
        }

        private void SetPage(int page)
        {
            PDFRenderer renderer = new PDFRenderer();

            int width = _document.GetPageWidth(_currentPage);
            int height = _document.GetPageHeight(_currentPage);

            _rawImage.texture = renderer.RenderPageToTexture(_document.GetPage(_currentPage), width, height);

            _aspectRatioFitter.aspectRatio = ((float) width / (float) height);
        }
    }
}