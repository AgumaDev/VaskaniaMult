using Unity.Netcode;
using UnityEngine;
public class DoorLogic : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Renderer door;

    public void TryOpenDoor()
    {
        OpenDoorServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void OpenDoorServerRpc()
    {
        OpenDoorClientRpc();
    }
    [Rpc(SendTo.Everyone)]
    private void OpenDoorClientRpc()
    {
        //anim.SetTrigger("Open");
        door.enabled = false;
    }
}
