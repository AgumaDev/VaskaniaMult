using System;
using UnityEngine;

public class FallingChandelierLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
