using TMPro;
using UnityEngine;

namespace _IExpo.Scripts.ExpoUI.PanelControllers
{
    public class ChatMessagesNotificationController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _notifications;

        public void SetUnreadCount(int count)
        {
            _notifications.text = count.ToString();
        }
    }
}