using _IExpo.Scripts.ExpoBackend.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _IExpo.Scripts.ExpoUI
{
    public class Registration : MonoBehaviour
    {
        [SerializeField] private UnityEvent SuccessRegistration;
        [SerializeField] private TMP_InputField _firstName;
        [SerializeField] private TMP_InputField _lastName;
        [SerializeField] private TMP_InputField _phone;
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _company;
        [SerializeField] private TMP_InputField _position;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TMP_InputField _password_confirmation;
        [SerializeField] private TMP_InputField _link;

        public void OnClickRegistration()
        {
            RegistrationHandler reg = new RegistrationHandler();
            UserInfo userInfo = new UserInfo(
                _firstName.text,
                _lastName.text,
                _phone.text,
                _email.text,
                _company.text,
                _position.text,
                _password.text,
                _password_confirmation.text,
                _link.text
            );

            StartCoroutine(reg.RegisterCoroutine(userInfo, SuccessRegistration ));
        }
    }
}