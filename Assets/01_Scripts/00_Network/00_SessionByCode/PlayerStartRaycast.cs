using UnityEngine;
using Unity.Netcode;

public class PlayerStartRaycast : NetworkBehaviour
{
    [SerializeField] private GameObject playerCameraObject;
    [SerializeField] private GameObject raycastStartPoint;

    void Update()
    {
        if (!IsOwner)
            return;
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(raycastStartPoint.transform.position, playerCameraObject.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                if (hit.collider.TryGetComponent<StartGameButton>(out var button))
                {
                    button.TryStartGame();
                }
            }
            
        }
    }
}
