using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class CandleSpawnerManager : NetworkBehaviour
{
    public GameObject candlePrefab;
    public List<Transform> spawnPoints;
    public VelaCounter velaCounter;
    public bool spawnedCandles;

    private void Update()
    {
        if (velaCounter != null && spawnedCandles) 
            UpdateClientVelaCounterRpc();
    }
    public void SpawnCandles()
    {
        foreach (Transform point in spawnPoints)
        {
            GameObject candle = Instantiate(candlePrefab, point.position, Quaternion.Euler(0, 0, 0));
            var netObj = candle.GetComponent<NetworkObject>();
            if (netObj != null)
                netObj.Spawn();
            
            spawnedCandles = true;
        }
    }
    [Rpc(SendTo.Everyone)]
    private void UpdateClientVelaCounterRpc()
    {
        velaCounter.enabled = true;
    }
}
