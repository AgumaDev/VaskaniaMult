using UnityEngine;
using Unity.Netcode;

public class RoomEvent : NetworkBehaviour
{
    [Header("## ROOM EVENT ##")]
    public ROOM_EVENT_TYPE roomEvent;
    public GameObject eventGameObjectRef;

    public enum ROOM_EVENT_TYPE
    {
        Door,
        Levitate,
        Teleport
    }

    public void TriggerEvent()
    {
        Debug.Log($"EVENT TRIGGERED: {roomEvent}");
        TriggerEventClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void TriggerEventClientRpc()
    {
        if (eventGameObjectRef != null)
            eventGameObjectRef.SetActive(true);
    }
}