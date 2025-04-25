using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ProtectionLogic : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PickUpController pickUpController;
    [SerializeField] public bool hasBeenUsed = false;

    private void OnEnable()
    {
        //anim.SetTrigger("Start");
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (hasBeenUsed)
        {
            OnProtectionRPC();
        }
    }
    
    [Rpc(SendTo.Server)]
    public void OnProtectionRPC()
    {
        pickUpController.rosary.enabled = false;
        pickUpController.cross.enabled = false;
        
        pickUpController.currentPickObject = null;
        pickUpController.pickedObject = false;
    }
}