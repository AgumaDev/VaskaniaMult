using Unity.Netcode;
using UnityEngine;
public class LighterLogic : NetworkBehaviour
{
    [SerializeField] private GameObject playerCameraObject;
    [SerializeField] private GameObject raycastStartPoint;

    public GameObject pointLight;
    public bool isLighted = false;

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
            ToggleLighterServerRpc(!isLighted);
        

        if (isLighted && Input.GetMouseButtonDown(1))
        {
            TryLightCandle();
        }
    }

    private void TryLightCandle()
    {
        Ray ray = new Ray(raycastStartPoint.transform.position, playerCameraObject.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.TryGetComponent(out NetworkObject netObj) && netObj.CompareTag("Vela"))
            {
                RequestLightCandleRpc(netObj.NetworkObjectId);
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestLightCandleRpc(ulong candleId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(candleId, out NetworkObject netObj))
        {
            if (netObj.TryGetComponent(out VelaPisoLogic vela))
            {
                vela.isLightenUp = true;
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void ToggleLighterServerRpc(bool turnOn)
    {
        ToggleLighterClientRpc(turnOn);
    }

    [Rpc(SendTo.Everyone)]
    private void ToggleLighterClientRpc(bool turnOn)
    {
        pointLight.SetActive(turnOn);
        isLighted = turnOn;
    }
}