using System;
using System.Collections;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI
{
    public class ControllableButton : MonoBehaviour, IControllable
    {
        [SerializeField] private GameObject _underlineObject;

        public void Activate()
        {
            _underlineObject.SetActive(true);
        }

        public void Deactivate()
        {
            _underlineObject.SetActive(false);
        }
    }

    public interface IControllable
    {
        void Activate();
        void Deactivate();
    }
}