using Unity.Services.Vivox;
using UnityEngine;

public class PlayerVoiceChat : MonoBehaviour
{
    public GameObject localPlayer;

    void Start()
    {
    
    }

    void Update()
    {
        VivoxService.Instance.Set3DPosition(localPlayer, SessionManager.instance.activeSession.Id.ToString());
    }
}
