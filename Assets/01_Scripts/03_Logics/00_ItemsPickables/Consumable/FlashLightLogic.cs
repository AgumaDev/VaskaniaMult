using Unity.Netcode;
using UnityEngine;
public class FlashLightLogic : NetworkBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public bool isEnabled;
    [SerializeField] public GameObject pointLight;
    private void OnEnable()
    {
        anim.SetTrigger("Oil Lamp");
    }
    private void Update()
    {
        if (!IsOwner) return;
        
            if (Input.GetMouseButtonDown(0))
            ToggleFlashlightServerRpc(!isEnabled);
    }
    
    [Rpc(SendTo.Server)]
    private void ToggleFlashlightServerRpc(bool turnOn)
    {
        ToggleFlashlightClientRpc(turnOn);
    }
    
    [Rpc(SendTo.Everyone)]
    private void ToggleFlashlightClientRpc(bool turnOn)
    {
        pointLight.SetActive(turnOn);
        isEnabled = turnOn;
    }
}
