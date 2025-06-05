using Unity.Netcode;
using UnityEngine;

public class PlayerVoiceChat : NetworkBehaviour
{
    void Start()
    {
        if (IsOwner && VoiceChatManager.instance != null)
        {
            VoiceChatManager.instance.SetLocalPlayer(gameObject);
            Debug.Log("Local Player set");
        }
    }
}