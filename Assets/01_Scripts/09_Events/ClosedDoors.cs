using System;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoors : MonoBehaviour
{
    public List<GameObject> lightsInRoom = new List<GameObject>(); 
    public List<GameObject> doorsInRoom = new List<GameObject>();

    public float deathCountdown;

    public GameObject nunSpawnPoint;
    public GameObject nun;
    
    //habria que ver el script de puerta y lockear el paso de la animacion, pero aun no hay puerta

    public bool isEventActive;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deathCountdown = deathCountdown - Time.deltaTime;

        if (deathCountdown < 0)
        {
            
        }
    }

    private void OnEnable()
    {
        deathCountdown = 20;
        for (int i = 0; i < lightsInRoom.Count; i++)
        {
            lightsInRoom[i].SetActive(false);
        }

        for (int i = 0; i < doorsInRoom.Count; i++)
        {
            doorsInRoom[i].SetActive(true);
        }
    }
}
