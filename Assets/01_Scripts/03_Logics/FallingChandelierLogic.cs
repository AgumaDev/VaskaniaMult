using System;
using UnityEngine;

public class FallingChandelierLogic : MonoBehaviour
{
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
