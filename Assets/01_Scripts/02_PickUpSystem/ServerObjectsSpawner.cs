using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerObjectsSpawner : NetworkBehaviour
{
    public List<GameObject> consumableItems = new List<GameObject>();
    public List<GameObject> consumableRoomList = new List<GameObject>();
    public List<GameObject> coreItems = new List<GameObject>();
    public List<GameObject> coreLocation = new List<GameObject>();

    public int maxItemCount;
    public int coreItemCount;

    public List<GameObject> selectedCoreItems = new List<GameObject>();

    private int clientsLoaded = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.SceneManager.OnSceneEvent += HandleSceneEvent;
    }

    private void HandleSceneEvent(SceneEvent sceneEvent)
    {
        if (sceneEvent.SceneEventType == SceneEventType.LoadComplete)
        {
            if (sceneEvent.ClientId != NetworkManager.ServerClientId)
            {
                clientsLoaded++;
            }

            int totalClients = NetworkManager.Singleton.ConnectedClients.Count - 1;
            if (clientsLoaded >= totalClients)
            {
                SpawnKeyItems();
                SpawnConsumableItems();

                // Evitar doble ejecución
                NetworkManager.Singleton.SceneManager.OnSceneEvent -= HandleSceneEvent;
            }
        }
    }
    void SpawnConsumableItems()
    {
        int pastItemNumber = -1;
        int pastRoomNumber = -1;

        for (int i = 0; i < maxItemCount; i++)
        {
            int currentRoomNumber = Random.Range(0, consumableRoomList.Count);
            int currentItemNumber = Random.Range(0, consumableItems.Count);

            if (currentItemNumber == pastItemNumber && currentRoomNumber == pastRoomNumber)
            {
                currentRoomNumber = Random.Range(0, consumableRoomList.Count);
                currentItemNumber = Random.Range(0, consumableItems.Count);
            }

            Vector3 randomOffset = new Vector3(Random.Range(-3, 4), 1, Random.Range(-3, 4));
            Vector3 spawnPosition = consumableRoomList[currentRoomNumber].transform.position + randomOffset;

            GameObject item = Instantiate(consumableItems[currentItemNumber], spawnPosition, Quaternion.Euler(0, 0, 0));
            var netObj = item.GetComponent<NetworkObject>();
            if (netObj != null)
                netObj.Spawn();

            pastItemNumber = currentItemNumber;
            pastRoomNumber = currentRoomNumber;
        }
    }
    void SpawnKeyItems()
    {
        for (int i = 0; i < coreItemCount; i++)
        {            
            int coreItemIndex = Random.Range(0, coreItems.Count);
            GameObject selectedItem = coreItems[coreItemIndex];
            coreItems.RemoveAt(coreItemIndex); // Evita repetir

            int locationIndex = Random.Range(0, coreLocation.Count);
            Vector3 spawnPosition = coreLocation[locationIndex].transform.position;

            GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.Euler(-90, 0, 0));
            
            var netObj = item.GetComponent<NetworkObject>();
            if (netObj != null)
                netObj.Spawn();

            selectedCoreItems.Add(selectedItem);
        }
    }
   
}
