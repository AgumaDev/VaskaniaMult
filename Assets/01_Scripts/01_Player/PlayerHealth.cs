using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] ProtectionLogic protectionLogic;
    [SerializeField] PickUpController pickUpController;
    private void Update()
    {
        if (!IsOwner) return; 
    }
    private void OnTriggerEnter(Collider other)
    {
        // if (!pickUpController.crossBool && !pickUpController.rosaryBool && other.CompareTag("DANGER"))
        //     DespawnPlayerRPC();
        // else if (pickUpController.crossBool || pickUpController.rosaryBool && other.CompareTag("DANGER"))
        //     protectionLogic.hasBeenUsed = true;

        if (other.CompareTag("DANGER"))
        {
           DespawnPlayerRPC();
        }
    }
    
    [Rpc(SendTo.Server)]
    private void DespawnPlayerRPC()
    {
        NetworkObject.Despawn();
    }
}
