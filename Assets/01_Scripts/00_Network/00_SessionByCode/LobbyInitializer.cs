using UnityEngine;
using Unity.Netcode;
public class LobbyInitializer : MonoBehaviour
{
    async void Start()
    {
        var intent = SessionManager.Instance.PendingIntent;

        if (intent == SessionIntent.Host)
        {
            bool success = await SessionManager.Instance.CreateSession();
            if (success)
            {
                NetworkManager.Singleton.StartHost();
            }
        }
        else if (intent == SessionIntent.Client)
        {
            bool success = await SessionManager.Instance.JoinSession(SessionManager.Instance.PendingJoinCode);
            if (success)
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}