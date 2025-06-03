using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WeepingAngel : MonoBehaviour
{
    public List<GameObject> playerList = new List<GameObject>();

    public RaycastHit hit;

    public bool canMove;
    public bool isVisible;
    public Renderer estatua;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = playerList[0].transform.position;

        Debug.DrawRay(transform.position, GetComponentInChildren<LookAtPlayer>().transform.forward, Color.yellow);

        if (canMove)
            GetComponent<NavMeshAgent>().speed = 3.5f;
        else
            GetComponent<NavMeshAgent>().speed = 0f;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (GeometryUtility.TestPlanesAABB(planes, estatua.bounds))
        { 
            Physics.Raycast(transform.position, GetComponentInChildren<LookAtPlayer>().transform.forward, out hit, 1000);
            {
                if (hit.collider == null || hit.collider.transform.CompareTag("Untagged"))
                {
                    canMove = true;
                }
                if (hit.collider.transform.CompareTag("Player"))
                {
                    canMove = false;
                }
            }

        }
        if (!GeometryUtility.TestPlanesAABB(planes, estatua.bounds))
        { 
            canMove = true;
        }
    }

    private void OnEnable()
    {
        playerList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
}
