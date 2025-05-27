using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class CandleSpawnerManager : NetworkBehaviour
{
    public VelaCounter velaCounter;

    private void Update()
    {
        if (velaCounter != null) 
            UpdateClientVelaCounterRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void UpdateClientVelaCounterRpc()
    {
        velaCounter.enabled = true;
    }
}
