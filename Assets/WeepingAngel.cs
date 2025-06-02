using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour
{
    public List<GameObject> playerList = new List<GameObject>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = playerList[0].transform.position;
        
        
        if (GetComponent<Renderer>().isVisible)
        {
            GetComponent<NavMeshAgent>().speed = 0;
        }
        else
        {
            GetComponent<NavMeshAgent>().speed = 3.5f;
        }
    }

    private void OnEnable()
    {
        playerList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
}
