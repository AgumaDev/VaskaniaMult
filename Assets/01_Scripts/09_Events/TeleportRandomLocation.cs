using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeleportRandomLocation : MonoBehaviour
{
    // cambie de teletransportar afuera a teletransportar a un lugar "aleatorio", creo que es mas interesante y menos tedioso

    public List<GameObject> playersInside = new List<GameObject>();

    public List<GameObject> possibleTeleportLocations = new List<GameObject>();

    public bool hasTeleported;
    
    void Update()
    {
        if (hasTeleported == false)
        {
            for (int i = 0; i < playersInside.Count; i++)
            {
                //posiblemente para esto queremos algo que le tape los ojos al jugador
                //se me ocurre que podriamos usar las manos de la monja que tapen la camara y luego la destapen en otro lugar
                
                
                playersInside[i].GetComponent<PlayerController>().enabled = false;
                playersInside[i].GetComponent<CharacterController>().enabled = false;
                playersInside[i].transform.position = possibleTeleportLocations[0].transform.position;
                playersInside[i].transform.position = possibleTeleportLocations[Random.Range(0, possibleTeleportLocations.Count)].transform.position;
                playersInside[i].GetComponent<CharacterController>().enabled = true;
                playersInside[i].GetComponent<PlayerController>().enabled = true;
                playersInside.Remove(playersInside[i]);
            }

            hasTeleported = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playersInside.Add(other.gameObject);
        }
    }
}
