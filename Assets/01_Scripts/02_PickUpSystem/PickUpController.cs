using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode.Components;
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

    [SerializeField] private ObjectsInHand objectsInHand;
    [SerializeField] public bool isProtected;

    private Dictionary<string, System.Action> pickUpActions;
    private Dictionary<string, System.Action> dropActions;

    private void Awake()
    {
        pickUpActions = new Dictionary<string, System.Action>
        {
            { "Core/Chalice Wine", () => { objectsInHand.chaliceWine.enabled = true; objectsInHand.wineLiquid.enabled = true; } },
            { "Core/Chalice Ostia", () => { objectsInHand.chaliceOstia.enabled = true; } },
            { "Core/Incense", () => { objectsInHand.incense.enabled = true; incenseLogic.enabled = true; incenseLogic.vfxSmoke.gameObject.SetActive(true); } },
            { "Core/Lighter", () => { objectsInHand.lighter.enabled = true; encendedorLogic.enabled = true; } },
            { "Core/Ashes", () => { objectsInHand.ashes.enabled = true; cenizasLogic.enabled = true; } },
            { "Core/Bell", () => { objectsInHand.bell.enabled = true; } },
            { "Core/Frame Painting", () => { objectsInHand.paintingFrame.enabled = true; } },
            { "Core/Pennant", () => { objectsInHand.pennant.enabled = true; } },
            { "Core/Salt", () => { objectsInHand.salt.enabled = true; } },
            { "Core/Holy Water", () => { objectsInHand.holyWater.enabled = true; objectsInHand.waterLiquid.enabled = true; objectsInHand.corcho.enabled = true; } },
            { "Consumable/Holy Wood", () => { objectsInHand.holyWood.enabled = true; paloSantoLogic.enabled = true; } },
            { "Consumable/Oil Lamp", () => { objectsInHand.oilLamp.enabled = true; flashLightLogic.enabled = true; } },
            { "Consumable/Ouija", () => { objectsInHand.ouija.enabled = true; ouijaLogic.enabled = true; } },
            { "Consumable/Cross", () => { objectsInHand.cross.enabled = true; crossLogic.enabled = true; isProtected = true; playerHealth.protectionLogic = crossLogic; }},
            { "Consumable/Rosary", () => { objectsInHand.rosary.enabled = true; rosaryLogic.enabled = true; isProtected = true; playerHealth.protectionLogic = rosaryLogic; }},
            { "Consumable/Key", () => { objectsInHand.key.enabled = true; keysLogic.enabled = true; } }
        };

        dropActions = new Dictionary<string, System.Action>
        {
            { "Core/Lighter", () => { objectsInHand.lighter.enabled = false; encendedorLogic.enabled = false;
                if (!pickedObject)
                {
                    encendedorLogic.pointLight.SetActive(false);
                    encendedorLogic.isLighted = false;
                }
            }},
            { "Core/Ashes", () => { objectsInHand.ashes.enabled = false; cenizasLogic.enabled = false; } },
            { "Core/Incense", () => { objectsInHand.incense.enabled = false; incenseLogic.enabled = false;
                if (!pickedObject)
                {
                    incenseLogic.vfxSmoke.gameObject.SetActive(false);
                    incenseLogic.isEnabled = false;
                } }},
            { "Core/Bell", () => { objectsInHand.bell.enabled = false; } },
            { "Core/Frame Painting", () => { objectsInHand.paintingFrame.enabled = false; } },
            { "Core/Pennant", () => { objectsInHand.pennant.enabled = false; } },
            { "Core/Salt", () => { objectsInHand.salt.enabled = false; } },
            { "Core/Holy Water", () => { objectsInHand.holyWater.enabled = false; objectsInHand.waterLiquid.enabled = false; objectsInHand.corcho.enabled = false; } },
            { "Core/Chalice Wine", () => { objectsInHand.chaliceWine.enabled = false; objectsInHand.wineLiquid.enabled = false; } },
            { "Core/Chalice Ostia", () => { objectsInHand.chaliceOstia.enabled = false; } },
            { "Consumable/Holy Wood", () => { objectsInHand.holyWood.enabled = false; paloSantoLogic.enabled = false; } },
            { "Consumable/Key", () => { objectsInHand.key.enabled = false; keysLogic.enabled = false; } },
            { "Consumable/Oil Lamp", () => { objectsInHand.oilLamp.enabled = false; flashLightLogic.enabled = false;
                if (!pickedObject)
                {
                    flashLightLogic.pointLight1.SetActive(false);
                    flashLightLogic.pointLight2.SetActive(false);
                } }},
            { "Consumable/Ouija", () => { objectsInHand.ouija.enabled = false; ouijaLogic.enabled = false; } },
            { "Consumable/Cross", () => { objectsInHand.cross.enabled = false; crossLogic.enabled = false; isProtected = false; }},
            { "Consumable/Rosary", () => { objectsInHand.rosary.enabled = false; rosaryLogic.enabled = false; isProtected = false; }} };
    }

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

        SetRenderersActive(obj.gameObject, false);
        
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<NetworkTransform>().InLocalSpace = false;
        obj.GetComponent<Collider>().enabled = false;

        if (pickUpActions.TryGetValue(obj.gameObject.tag, out var action))
            action.Invoke();

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

        pickedObject = false;

        foreach (var action in dropActions.Values)
            action.Invoke();

        SetRenderersActive(obj.gameObject, true);
        obj.transform.position = dropPos;
        obj.GetComponent<Collider>().enabled = true;

        var rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        obj.GetComponent<NetworkTransform>().InLocalSpace = true;

        if (IsOwner)
            currentPickedObject = null;
    }
    private void SetRenderersActive(GameObject obj, bool state)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
            renderer.enabled = state;
    }
}