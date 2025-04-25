using Unity.Netcode;
using UnityEngine;
public class CenizasLogic : NetworkBehaviour
{
    [SerializeField] private GameObject playerCameraObject;
    [SerializeField] private GameObject raycastStartPoint;
    
    [SerializeField] private Animator anim;

    private void OnEnable()
    {
        //anim.SetTrigger("Cenizas");
    }
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceAshes();
        }
    }

    private void TryPlaceAshes()
    {
        Ray ray = new Ray(raycastStartPoint.transform.position, playerCameraObject.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Vector3 point = hit.point;
            Vector3 normal = hit.normal;
            bool isMainArea = hit.transform.CompareTag("MainAreaWalls");

            // Llama al DecalManager para que se comparta el array entre clientes
            DecalManager.Instance.PlaceDecalRpc(point, normal, isMainArea);
        }
    }
}