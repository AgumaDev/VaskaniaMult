using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityLogic : MonoBehaviour
{
    //logica de speedup?

    public List<GameObject> playerList = new List<GameObject>();

    public float chaseSpeed;
    
    public float baseSpeed;
    public float firstSpeedUp;
    public float secondSpeedUp;

    public float totalChaseTime;

    public float firstSpeedUpTimer;
    public float secondSpeedUpTimer;

    public int currentLifePoints;
    public int lifePoints;

    public GameObject respawnPoint;

    public AudioRecognition audioRecognition;
    public bool hasTeleported;
    
    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        playerList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        chaseSpeed = baseSpeed;
        lifePoints = 3;
        audioRecognition.entity[0] = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
        totalChaseTime += Time.deltaTime;
        
        GetComponent<NavMeshAgent>().speed = chaseSpeed;
        GetComponent<NavMeshAgent>().destination = playerList[0].transform.position;

        if (totalChaseTime > firstSpeedUpTimer && totalChaseTime < secondSpeedUpTimer)
        {
            chaseSpeed = firstSpeedUp;
        }

        if (totalChaseTime > secondSpeedUpTimer)
        {
            chaseSpeed = secondSpeedUp;
        }

        if (lifePoints == 2 && hasTeleported || lifePoints == 1 && hasTeleported)
        {
            chaseSpeed = baseSpeed;
            transform.position = respawnPoint.transform.position;
            totalChaseTime = 0;
            hasTeleported = false;
        }

        if(lifePoints == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
