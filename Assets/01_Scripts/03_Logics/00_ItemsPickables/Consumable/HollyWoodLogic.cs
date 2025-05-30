using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class HollyWoodLogic : NetworkBehaviour
{
    [SerializeField] public RoomSystem roomSystem;
    [SerializeField] private ObjectsInHand objectsInHand;
    [SerializeField] public PickUpController pickUpController;

    [SerializeField] private DissolveController dissolveController;
    private void OnEnable()
    {
        roomSystem = FindFirstObjectByType<RoomSystem>();
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
            TryInvisibility();
    }
    private void TryInvisibility()
    {
        if (roomSystem.inRoom.Value)
            ProtectionRpc();
        else
            Debug.Log("not in room");
    }
    
    [Rpc(SendTo.Server)]
    private void ProtectionRpc()
    {
        StartCoroutine(ProtectionCoolDown());
    }
    IEnumerator ProtectionCoolDown()
    {
        pickUpController.isProtected = true;
        Debug.Log("Protection started");
        
        yield return new WaitForSeconds(5);
        
        yield return StartCoroutine(dissolveController.DissolveCo());
        DisablePaloSantoRpc();
        Debug.Log("Protection ended");
    }

    [Rpc(SendTo.Everyone)]
    private void DisablePaloSantoRpc()
    {
        pickUpController.isProtected = false;
        objectsInHand.holyWood.enabled = false;
        
        pickUpController.currentPickedObject = null;
        pickUpController.pickedObject = false;
    }
}
