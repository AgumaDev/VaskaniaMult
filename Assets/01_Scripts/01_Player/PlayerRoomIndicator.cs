using Unity.Netcode;
using UnityEngine;

public class PlayerRoomIndicator : NetworkBehaviour
{
    public RoomSystem insideRoomSystem;
    public bool inRoom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room") && !inRoom)
        {
            inRoom = true;

            insideRoomSystem = other.GetComponent<RoomSystem>();
            NetworkObjectReference player = new NetworkObjectReference(NetworkObject);
            insideRoomSystem.AddPlayerToRoomRpc(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room") && inRoom)
        {
            inRoom = false;
            
            NetworkObjectReference player = new NetworkObjectReference(NetworkObject);
            insideRoomSystem.RemovePlayerFromRoomRpc(player);
            
            insideRoomSystem = null;
        }
    }
}