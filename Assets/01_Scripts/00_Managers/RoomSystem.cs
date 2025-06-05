using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoomSystem : NetworkBehaviour
{
    [Header("## ROOM SYSTEM ##")]
    [Space(5f)]
    
    [SerializeField] private float timeToEvent;
    private float timerToEvent;
    private bool eventTriggered;

    [Header("EVENT DATA")]
    [Space(5f)]
    
    [SerializeField] private int baseEventChance;
    [SerializeField] private int bonusEventChance;
    [SerializeField] private float bonusEventChanceMultiplier;
    [SerializeField] private float eventCoolDown;
    private int CurrentEventChance() => baseEventChance + bonusEventChance;

    [Header("PLAYER RELATED")]
    [Space(5f)]

    public List<GameObject> playersInRoom;

    [Header("ROOM EVENTS")]
    [Space(5f)]

    [SerializeField] private List<RoomEvent> roomEvents = new();

    private void Awake()
    {
        roomEvents.Clear();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out RoomEvent roomEvent))
                roomEvents.Add(roomEvent);
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        if (playersInRoom.Count > 0 && roomEvents.Count > 0 && !eventTriggered)
        {
            timerToEvent += Time.deltaTime;
            if (timerToEvent >= timeToEvent)
            {
                timerToEvent = 0;
                CheckEventChance();
            }
        }
    }

    private void CheckEventChance()
    {
        if (Random.Range(0, 100) < CurrentEventChance())
            TriggerRandomEvent();
        else
            bonusEventChance += Mathf.RoundToInt(baseEventChance / bonusEventChanceMultiplier);
    }

    private void TriggerRandomEvent()
    {
        int randomIndex = Random.Range(0, roomEvents.Count);
        RoomEvent selectedEvent = roomEvents[randomIndex];

        selectedEvent.TriggerEvent();
        roomEvents.RemoveAt(randomIndex);
        bonusEventChance = 0;
        eventTriggered = true;

        StartCoroutine(EventCoolDown());
    }

    private IEnumerator EventCoolDown()
    {
        yield return new WaitForSeconds(eventCoolDown); // cambiar si necesitamos más tiempo entre eventos
        eventTriggered = false;
    }

    [Rpc(SendTo.Server)]
    public void AddPlayerToRoomRpc(NetworkObjectReference player)
    {
        if (!playersInRoom.Contains(player))
            playersInRoom.Add(player);
    }

    [Rpc(SendTo.Server)]
    public void RemovePlayerFromRoomRpc(NetworkObjectReference player)
    {
        playersInRoom.Remove(player);
    }
}