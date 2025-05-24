using Unity.Netcode;
using System.Collections;
using UnityEngine;
public class ProtectionLogic : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PickUpController pickUpController;
    [SerializeField] private DissolveController dissolveController;

    [SerializeField] private ObjectsInHand objectsInHand;
    private void OnEnable()
    {
        //anim.SetTrigger("Start");
    }
    public void OnProtectionServer()
    {
        if (!IsServer) return;

        UpdateItemRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void UpdateItemRpc()
    {
        StartCoroutine(HandleDissolveAndDisable());
    }

    private IEnumerator HandleDissolveAndDisable()
    {
        yield return StartCoroutine(dissolveController.DissolveCo());

        objectsInHand.rosary.enabled = false;
        objectsInHand.cross.enabled = false;
        pickUpController.isProtected = false;

        pickUpController.currentPickedObject = null;
        pickUpController.pickedObject = false;

        //dissolveController.dissolveMat.SetFloat("_DissolveAmount", 0f);
    }
}