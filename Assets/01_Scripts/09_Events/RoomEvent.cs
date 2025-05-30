using UnityEngine;
using Unity.Netcode;
public class RoomEvent : NetworkBehaviour
{
    [Header("## ROOM EVENT ##")]
    [Space(5)]

    [Header("EVENT")]
    public ROOM_EVENT_TYPE roomEvent;
    public GameObject eventGameObjectRef;
    public enum ROOM_EVENT_TYPE
    {
        Door,
        Levitate
    }
    
    [Rpc(SendTo.Server)]
    public void RequestTriggerEventServerRpc()
    {
        Debug.Log($"EVENT TRIGGERED: {roomEvent}");
        TriggerEventClientRpc();
    }
    
    [Rpc(SendTo.Everyone)]
    void TriggerEventClientRpc()
    {
        eventGameObjectRef.SetActive(true);
    }

}
