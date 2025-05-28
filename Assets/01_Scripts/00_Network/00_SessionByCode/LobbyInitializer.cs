using UnityEngine;
using Unity.Netcode;
public class LobbyInitializer : MonoBehaviour
{
    async void Start()
    {
        var intent = SessionManager.instance.PendingIntent;

        if (intent == SessionIntent.Host)
        {
            bool success = await SessionManager.instance.CreateSession();
            if (success)
            {
                NetworkManager.Singleton.StartHost();
            }
        }
        else if (intent == SessionIntent.Client)
        {
            bool success = await SessionManager.instance.JoinSession(SessionManager.instance.PendingJoinCode);
            if (success)
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}