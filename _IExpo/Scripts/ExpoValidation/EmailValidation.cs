using SpaceHub.Conference;
using System;
using System.Collections;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace _IExpo.Scripts.ExpoValidation
{
    public class EmailValidation : ValidationBase
    {
        public override void IsFieldValid(UnityAction callbackIfValid, UnityAction callbackIfNotValid)
        {
            if (IsEmailValid() && ThisField.text.Length >= MinLength)
            {
                StopAllCoroutines();
                StartCoroutine(IsEmailUnique(ThisField.text, callbackIfValid, callbackIfNotValid));
            }
            else
            {
                ShowWarning();
                callbackIfNotValid();
            }
        }
        private bool IsEmailValid()
        {
            try
            {
                MailAddress m = new MailAddress(ThisField.text);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public override void ShowWarning()
        {
            if (ThisField.text.Length < MinLength)
            {
                ShowWarningOfType(WarningsEnum.MinLength);
                return;
            }
            else if (!IsEmailValid())
            {
                ShowWarningOfType(WarningsEnum.WrongEmailFormat);
                return;
            }
            else
            {
                ShowWarningOfType(WarningsEnum.NotUniqueEmail);
                return;
            }
        }

        private IEnumerator IsEmailUnique(string email, UnityAction callbackIfValid, UnityAction callbackIfNotValid)
        {
            WWWForm form = new WWWForm();

            form.AddField("email", email);

            using (UnityWebRequest webRequest = UnityWebRequest.Post(ConferenceServerSettings.Instance.GetApiRoute("isemailunique"), form))
            {
                webRequest.SetRequestHeader("Token", SpaceHubLogin.Token);

                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogWarning($"Error: {webRequest.error}");
                    callbackIfNotValid();
                }
                else
                {
                    CheckUniqueEmail checkEmail = JsonUtility.FromJson<CheckUniqueEmail>(webRequest.downloadHandler.text);
                    if (checkEmail.ResultCode == "0" && checkEmail.Message == "successful" && checkEmail.IsEmailUnique == "true")
                    {
                        callbackIfValid();
                    }
                    else
                    {
                        ShowWarning();
                        callbackIfNotValid();
                    }
                }
            }
        }
    }

    public class CheckUniqueEmail
    {
        public string IsEmailUnique;
        public string ResultCode;
        public string Message;
    }
}
