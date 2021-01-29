using System;
using System.Collections;
using SpaceHub.Conference;
using UnityEngine;

namespace _IExpo.Scripts.ExpoStand
{
    public class WaitForPhoton
    {
        public IEnumerator Start(Action afterComplete)
        {
            var waitSeconds = new WaitForSeconds(0.1f);

            yield return new WaitForSeconds(0.2f);

            while (LocalPlayerIsNotConnected)
            {
                yield return waitSeconds;
            }

            afterComplete();
        }

        private static bool LocalPlayerIsNotConnected =>
            PlayerLocal.Instance == null ||
            PlayerLocal.Instance.GetPlayer() == null ||
            PlayerLocal.Instance.CustomProperties == null
            || !PlayerLocal.Instance.Client.IsConnectedAndReady;
    }
}