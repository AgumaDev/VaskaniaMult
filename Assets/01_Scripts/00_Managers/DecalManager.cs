using Unity.Netcode;
using UnityEngine;
public class DecalManager : NetworkBehaviour
{
    public static DecalManager Instance;

    [SerializeField] private GameObject decalPrefab;
    [SerializeField] private NetworkObject[] decalArray;
    public int decalsInMainArea;
    public bool hasSentData;
    
    private int numberOfDecals;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Rpc(SendTo.Server)]
    public void PlaceDecalRpc(Vector3 point, Vector3 normal, bool isMainArea)
    {
        GameObject newDecal = Instantiate(decalPrefab, point, Quaternion.LookRotation(-normal));
        var netObj = newDecal.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
            UpdateDecalListClientRpc(netObj.NetworkObjectId, isMainArea);
        }
    }
    [Rpc(SendTo.Server)]
    public void DespawnDecalRpc()
    {
        decalArray[numberOfDecals].Despawn();
    }

    [Rpc(SendTo.Everyone)]
    private void UpdateDecalListClientRpc(ulong netId, bool isMainArea)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(netId, out var netObj))
            return;

        if (decalArray[numberOfDecals] != null)
        {
            DespawnDecalRpc();
        }

        decalArray[numberOfDecals] = netObj;
        decalArray[numberOfDecals].GetComponent<DecalCounter>().isInsideDecal = isMainArea ? 1 : 0;

        numberOfDecals = (numberOfDecals + 1) % decalArray.Length;

        decalsInMainArea = 0;
        foreach (var decal in decalArray)
        {
            if (decal != null)
            {
                decalsInMainArea += decal.GetComponent<DecalCounter>().isInsideDecal;
            }
        }

        if (decalsInMainArea >= 3 && !hasSentData)
        {
            ItemRecognitionArea._instance.DecalCounterUp();
            hasSentData = true;
            print("poggies");
        }

        if (decalsInMainArea < 3 && hasSentData)
        {
            ItemRecognitionArea._instance.DecalCounterDown();
            hasSentData = false;
        }
    }
}
