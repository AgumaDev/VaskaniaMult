using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class RoomSystem : NetworkBehaviour
{
    [Header("## ROOM SYSTEM ##")]
    [Space(5)]

    [Header("EVENT")]
    [SerializeField] float timeToEvent;
    float timerToEvent;

    [Header("EVENT CHANCE")]
    public int baseEventChance; //PROBABILIDAD BASE
    public int bonusEventChance; //PROBABILIDAD ACUMULADA EN BASE A FALLOS
    public float bonusEventChanceMultiplier; //MULTIPLICADOR EN CASO DE 

    //public TextMeshProUGUI datos;
    int CurrentEventChange() {  return baseEventChance + bonusEventChance; }

    [Header("PLAYER RELATED")]
    [SerializeField] public NetworkVariable<bool> inRoom;
    [SerializeField] private List<GameObject> playersInRoom = new();

    [Header("ROOM EVENTS")]
    public List<RoomEvent> roomEvents = new();

    void Awake()
    {
        roomEvents.Clear();
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out RoomEvent roomEvent))
            {
                roomEvents.Add(roomEvent);
            }
        }
    }

    private void Update()
    {     
        if(playersInRoom.Count >= 1 && roomEvents.Count > 0)
        {
            timerToEvent += Time.deltaTime;
            if(timerToEvent >= timeToEvent)
            {
                timerToEvent = 0; //¿COOLDOWN?
                CheckEventChance();
            }
        }

        //datos.text = "TAB to open book" + "<br><br>" + "Players In Room = " + playerInRoom +  "<br>" + "Base event chance = " + baseEventChance + "<br>" +
                    // "Bonus event chance= " + bonusEventChance + "<br>" + "Total Event Chance = " + (baseEventChance +
                     //    bonusEventChance) + "<br>" +"Timer for event = " + timerToEvent; 
    }

    public void ItemSpawning()
    { 
        
    }

    public void CheckEventChance()
    {
        if(Random.Range(0, 100) < CurrentEventChange())
            TriggerRandomEvent();
        else
        {
            //??
            bonusEventChance += Mathf.RoundToInt(baseEventChance / bonusEventChanceMultiplier);
        }
    }

    void TriggerRandomEvent()
    {
        bonusEventChance = 0;
        int randomEventIndex = Random.Range(0, roomEvents.Count);
        roomEvents[randomEventIndex].RequestTriggerEventServerRpc();
        roomEvents.RemoveAt(randomEventIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            playersInRoom.Add(other.gameObject);
            inRoom.Value = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playersInRoom.Remove(other.gameObject);
            inRoom.Value = false;
        }
    }
}
