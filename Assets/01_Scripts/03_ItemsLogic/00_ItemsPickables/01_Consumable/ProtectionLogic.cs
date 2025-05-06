using Unity.Netcode;
using UnityEngine;
public class ProtectionLogic : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PickUpController pickUpController;
    private void OnEnable()
    {
        //anim.SetTrigger("Start");
    }
    
    [Rpc(SendTo.Everyone)]
    private void UpdateItemRpc()
    {
        pickUpController.rosary.enabled = false;
        pickUpController.cross.enabled = false;
        pickUpController.isProtected = false;

        pickUpController.currentPickedObject = null;
        pickUpController.pickedObject = false;
    }
    public void OnProtectionServer()
    {
        if (!IsServer) return;

        UpdateItemRpc();
    }
}