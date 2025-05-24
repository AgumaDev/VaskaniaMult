using System;
using UnityEngine;

public class FallingChandelierTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
