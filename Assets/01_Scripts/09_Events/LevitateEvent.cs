using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class LevitateEvent : MonoBehaviour
{
    public float levitationRbForce = 2f;
    public float levitationPcForce = 2f;
    public List<Rigidbody> objectsInside;
    public List<PlayerController> playersInside;

    private void OnTriggerStay(Collider other)
    {
        var rb = other.attachedRigidbody;
        var pc = other.GetComponent<PlayerController>();

        if (rb != null && !objectsInside.Contains(rb))
        {
            rb.useGravity = false;
            rb.AddForce(Vector3.up * levitationRbForce, ForceMode.Acceleration);
            objectsInside.Add(rb);
        }
        
        if (pc != null && !playersInside.Contains(pc))
        {
            pc.gravity = levitationPcForce;
            playersInside.Add(pc);
            pc.ResetVerticalVelocity(); 
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        var rb = other.attachedRigidbody;
        var pc = other.GetComponent<PlayerController>();

        if (rb != null && !objectsInside.Contains(rb))
        {
            rb.useGravity = false;
            objectsInside.Add(rb);
        }

        if (pc != null && !playersInside.Contains(pc))
        {
            pc.gravity = levitationPcForce;
            playersInside.Add(pc);
            pc.ResetVerticalVelocity(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.attachedRigidbody;
        var pc = other.GetComponent<PlayerController>();

        if (rb != null && objectsInside.Contains(rb))
        {
            rb.useGravity = true;
            objectsInside.Remove(rb);
        }
        
        if (pc != null)
        {
            pc.gravity = -9.8f;
            pc.ResetVerticalVelocity();
            
            if (playersInside.Contains(pc))
            {
                playersInside.Remove(pc);
            }
            
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody rb in objectsInside)
        {
            if (rb != null)
            {
                rb.AddForce(Vector3.up * levitationRbForce, ForceMode.Acceleration);
            }
        }
    }
}
