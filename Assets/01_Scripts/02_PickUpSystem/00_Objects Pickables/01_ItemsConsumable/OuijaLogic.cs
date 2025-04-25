using Unity.Netcode;
using UnityEngine;
public class OuijaLogic : NetworkBehaviour
{
    [SerializeField] Animator anim;
    private void OnEnable()
    {
        //anim.SetTrigger("Ouija");
    }
    private void Update()
    {
        if(!IsOwner) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            DespawnPlayerRpc();
        }
    }
    
    [Rpc(SendTo.Server)]
    private void DespawnPlayerRpc()
    {
        NetworkObject.Despawn();
    }
}
