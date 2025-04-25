using UnityEngine;
using Unity.Netcode;
public class PaloSantoLogic : NetworkBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public bool isEnabled;
    [SerializeField] public bool isInArea;
    [SerializeField] public bool hasBeenUsed;
    [SerializeField] public GameObject particles;
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
            ItemRecognitionArea.Instance.coreItemActivated++;
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
        particles.SetActive(turnOn);
        isEnabled = turnOn;
    }
}
