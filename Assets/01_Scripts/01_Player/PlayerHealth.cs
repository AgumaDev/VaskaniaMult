using System;
using Unity.Netcode;
using UnityEngine;
public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] public ProtectionLogic protectionLogic;
    [SerializeField] private PickUpController pickUpController;

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsOwner) return;

        if (other.transform.CompareTag("DANGER"))
            TryHandleDangerServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void TryHandleDangerServerRpc()
    {
        if (pickUpController == null || protectionLogic == null) return;

        if (pickUpController.isProtected)
            protectionLogic.OnProtectionServer();
        else
            DespawnPlayerRPC();
    }
    [Rpc(SendTo.Server)]
    private void DespawnPlayerRPC()
    {
        NetworkObject.Despawn();
    }
}
