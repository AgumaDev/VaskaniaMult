using System;
using System.Collections.Generic;
using UnityEngine;

public class ClosedDoors : MonoBehaviour
{
    public List<GameObject> lightsInRoom = new List<GameObject>(); 
    public List<GameObject> doorsInRoom = new List<GameObject>();
    
    //habria que ver el script de puerta y lockear el paso de la animacion, pero aun no hay puerta

    public bool isEventActive;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < lightsInRoom.Count; i++)
        {
            lightsInRoom[i].SetActive(false);
        }
    }
}
