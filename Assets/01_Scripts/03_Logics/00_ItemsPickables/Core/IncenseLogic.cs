using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

public class IncenseLogic : NetworkBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public bool isEnabled;
    [SerializeField] public bool isInArea;
    [SerializeField] public bool hasBeenUsed;
    [SerializeField] public VisualEffect vfxSmoke;
    [SerializeField] public PlayerController playerController;
    private void OnEnable()
    {
        // anim.SetTrigger("Palo Santo");
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            UsePaloServerRpc(!isEnabled);
        }

        if (playerController.insideMainArea)
            isInArea = true;
        else
            isInArea = false;

        if (isEnabled && isInArea && hasBeenUsed == false)
        {
            ItemRecognitionArea._instance.coreItemActivated.Value++;
            hasBeenUsed = true;
        }
    }
    [Rpc(SendTo.Server)]
    private void UsePaloServerRpc(bool turnOn)
    {
        UsePaloClientRpc(turnOn);
    }
    [Rpc(SendTo.Everyone)]
    private void UsePaloClientRpc(bool turnOn)
    {
        isEnabled = turnOn;
        if(isEnabled)
            vfxSmoke.Play();
        else
            vfxSmoke.Stop();
    }
}
