using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
public class PickUpController : NetworkBehaviour
{
    [Header("PickUp Logic")]

    [SerializeField] private GameObject playerCameraObject;
    [SerializeField] private GameObject raycastStartPoint;
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private LayerMask pickUpLayer;

    [SerializeField] public NetworkObject currentPickedObject;
    [SerializeField] public bool pickedObject;

    [Header("Picked Objects Logic")]
    [SerializeField] private FlashLightLogic flashLightLogic;
    [SerializeField] private OuijaLogic ouijaLogic;

    [SerializeField] private ProtectionLogic crossLogic;
    [SerializeField] private ProtectionLogic rosaryLogic;
    [SerializeField] private PaloSantoLogic paloSantoLogic;
    [SerializeField] private IncenseLogic incenseLogic;
    [SerializeField] private KeysLogic keysLogic;
    [SerializeField] private EncendedorLogic encendedorLogic;
    [SerializeField] private CenizasLogic cenizasLogic;
    public PlayerHealth playerHealth;

    [SerializeField] public Renderer calizHostia, calizVino, cenizas, oilLamp, ouija, encendedor, cross, rosary, incienso, key, sal, aguaBendita, banderin, campanilla, cuadro, paloSanto;
    [SerializeField] public bool isProtected;
    void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!pickedObject)
                TryPickUp();
            else
                DropObject();
        }
    }
    private void TryPickUp()
    {
        Ray ray = new Ray(raycastStartPoint.transform.position, playerCameraObject.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 3f, pickUpLayer))
        {
            NetworkObject netObj = hit.collider.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                RequestPickUpServerRpc(netObj.NetworkObjectId);
            }
        }
    }
    
    [Rpc(SendTo.Server)]
    private void RequestPickUpServerRpc(ulong objectId)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var obj)) return;
        UpdateClientsOnPickUpRpc(objectId);
    }
    
    [Rpc(SendTo.Everyone)]
    private void UpdateClientsOnPickUpRpc(ulong objectId)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var obj)) return;
        
        obj.GetComponent<Renderer>().enabled = false;
        obj.GetComponent<NetworkTransform>().InLocalSpace = false;
        obj.GetComponent<Collider>().enabled = false;
        
        if (obj.gameObject.CompareTag("Oil Lamp"))
        {
            oilLamp.enabled = true;
            flashLightLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("Ouija"))
        {
             ouija.enabled = true; 
             ouijaLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("Cross"))
        {
            cross.enabled = true; 
            crossLogic.enabled = true;      
            isProtected = true;
            playerHealth.protectionLogic = crossLogic;
        }
        if (obj.gameObject.CompareTag("Rosary"))
        {
            rosary.enabled = true; 
            rosaryLogic.enabled = true;
            isProtected = true;
            playerHealth.protectionLogic = rosaryLogic;
        }
        if (obj.gameObject.CompareTag("Incienso"))
        {
            incienso.enabled = true; 
            incenseLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("Key"))
        {
            key.enabled = true; 
            keysLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("Encendedor"))
        {
            encendedor.enabled = true; 
            encendedorLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("Cenizas"))
        {
            cenizas.enabled = true;
            cenizasLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("Campanillas"))
        {
            campanilla.enabled = true;
        }
        if (obj.gameObject.CompareTag("Cuadro"))
        {
            cuadro.enabled = true;
        }
        if (obj.gameObject.CompareTag("Banderin"))
        {
            banderin.enabled = true;
        }
        if (obj.gameObject.CompareTag("Sal"))
        {
            sal.enabled = true;
        }
        if (obj.gameObject.CompareTag("Palo Santo"))
        {
            paloSanto.enabled = true;
            paloSantoLogic.enabled = true;
        }
        if (obj.gameObject.CompareTag("AguaBendita"))
        {
            aguaBendita.enabled = true;
        }
        if (obj.gameObject.CompareTag("Caliz Hostia"))
        {
            calizHostia.enabled = true;
        }
        if (obj.gameObject.CompareTag("Caliz Vino"))
        {
            calizVino.enabled = true;
        }
        
        pickedObject = true;

        if (IsOwner)
            currentPickedObject = obj;
    }

    private void DropObject()
    {
        if (currentPickedObject == null) return;
        
        Vector3 dropPosition = pickUpPoint.position;
        RequestDropServerRpc(currentPickedObject.NetworkObjectId, dropPosition);
        
        currentPickedObject = null;
    }

    [Rpc(SendTo.Server)]
    private void RequestDropServerRpc(ulong objectId, Vector3 dropPos)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var obj)) return;

        obj.transform.position = dropPos;
        UpdateClientsOnDropRpc(objectId, dropPos);
    }

    [Rpc(SendTo.Everyone)]
    private void UpdateClientsOnDropRpc(ulong objectId, Vector3 dropPos)
    {
        if (!NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var obj)) return;
        
        // debe desactivar todos los objetos pq siempre se sueltan, independiente de cual haya sido recogido
        pickedObject = false;
   
        if (flashLightLogic != null && !pickedObject)
        {
            flashLightLogic.pointLight.SetActive(false);
            flashLightLogic.isEnabled = false;
        }
        oilLamp.enabled = false;
        flashLightLogic.enabled = false;
        
        ouija.enabled = false;
        ouijaLogic.enabled = false;

        if (encendedorLogic != null && !pickedObject)
        {
            encendedorLogic.pointLight.SetActive(false);
            encendedorLogic.isLighted = false;
        }
        encendedor.enabled = false; 
        encendedorLogic.enabled = false;

        cenizas.enabled = false;
        cenizasLogic.enabled = false;
        
        if (incenseLogic != null && !pickedObject)
        {
            incenseLogic.particles.SetActive(false);
            incenseLogic.isEnabled = false;
        }
        incienso.enabled = false;
        incenseLogic.enabled = false;

        key.enabled = false;
        keysLogic.enabled = false;
        
        calizHostia.enabled = false;
        
        calizVino.enabled = false;
        
        cross.enabled = false;
        crossLogic.enabled = false;
        isProtected = false;
        
        rosary.enabled = false;
        rosaryLogic.enabled = false;
        isProtected = false;

        paloSanto.enabled = false;
        paloSantoLogic.enabled = false;
        
        cuadro.enabled = false;
        
        campanilla.enabled = false;
        
        banderin.enabled = false;
        
        sal.enabled = false;
        
        aguaBendita.enabled = false;
        
        
        obj.transform.position = dropPos;
        obj.GetComponent<Collider>().enabled = true;
        obj.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
        obj.GetComponent<Renderer>().enabled = true;
        obj.GetComponent<NetworkTransform>().InLocalSpace = true;
        

        if (IsOwner)
        {
            currentPickedObject = null;
        }
    }
}