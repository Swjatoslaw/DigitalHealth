using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

namespace _IExpo.Scripts.ExpoUI
{
    [Serializable]
    public class GradientSlider
    {
        [SerializeField] private Slider _skinColorSlider;
        [SerializeField] private UIGradient _gradient;

        public Color GetCurrentColor()
        {
            return _gradient.EffectGradient.Evaluate(_skinColorSlider.value);
        }

        public void SetSliderValue(float newValue)
        {
            _skinColorSlider.value = Mathf.Clamp(newValue, 0f, 1f);
        }

        public float GetSliderValue()
        {
            return _skinColorSlider.value;
        }
    }
}