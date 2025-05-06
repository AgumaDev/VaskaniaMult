using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class KeysLogic : NetworkBehaviour
{
    [SerializeField] private GameObject playerCameraObject;
    [SerializeField] private GameObject raycastStartPoint;
    
    [SerializeField] private PickUpController pickUpController;
    [SerializeField] private Animator anim;
    public void OnEnable()
    {
        //anim.SetTrigger("Start");
    }
    private void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetMouseButtonDown(0))
            TryOpenDoor();
    }
    private void TryOpenDoor()
    {
        Ray ray = new Ray(raycastStartPoint.transform.position, playerCameraObject.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.TryGetComponent<DoorLogic>(out var door))
            {
                DisableKeyServerRpc();
                door.TryOpenDoor();
            }
        }
    }
    [Rpc(SendTo.Server)]
    private void DisableKeyServerRpc()
    {
        DisableKeyClientRpc();
    }
    [Rpc(SendTo.Everyone)]
    private void DisableKeyClientRpc()
    {
        pickUpController.key.enabled = false;
        pickUpController.currentPickedObject = null;
        pickUpController.pickedObject = false;
    }
}
