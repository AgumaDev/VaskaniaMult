using Unity.Netcode;
using UnityEngine;
public class VelaPisoLogic : NetworkBehaviour
{
    public bool isLightenUp;
    public bool hasSentData;

    public GameObject pointLight;

    void Update()
    {
        if (!IsOwner) return;

        if (isLightenUp && !hasSentData)
        {
            LightCandleRpc();
        }
    }
    
    [Rpc(SendTo.Everyone)]
    private void LightCandleRpc()
    {
        pointLight.SetActive(true);
        VelaCounter.Instance.VelaCount();
        hasSentData = true;
    }
}
