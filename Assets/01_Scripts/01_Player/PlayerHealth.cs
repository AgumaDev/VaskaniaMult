using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] public ProtectionLogic protectionLogic;
    [SerializeField] PickUpController pickUpController;
    private void Update()
    {
        if (!IsOwner) return; 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!pickUpController.isProtected && other.CompareTag("DANGER"))
            DespawnPlayerRPC();
        else if (pickUpController.isProtected && other.CompareTag("DANGER"))
            protectionLogic.hasBeenUsed = true;
    }
    
    [Rpc(SendTo.Server)]
    private void DespawnPlayerRPC()
    {
        NetworkObject.Despawn();
    }
}
