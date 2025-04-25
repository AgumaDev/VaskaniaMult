using UnityEngine;

public class RoomEvent : MonoBehaviour
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

    public void TriggerEvent()
    {
        print($"EVENT TRIGGERED: {roomEvent}");
        eventGameObjectRef.SetActive(true);
    }
}
