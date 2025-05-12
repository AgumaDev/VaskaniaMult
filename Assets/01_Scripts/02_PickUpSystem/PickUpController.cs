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

    [SerializeField] public Renderer calizHostia, calizVino, liquidVino, cenizas, oilLamp, ouija,
        encendedor, cross, rosary, incienso, key, sal, aguaBendita, banderin, campanilla, cuadro, paloSanto;

    [SerializeField] public bool isProtected;

    private Dictionary<string, System.Action> pickUpActions;
    private Dictionary<string, System.Action> dropActions;

    private void Awake()
    {
        pickUpActions = new Dictionary<string, System.Action>
        {
            { "Caliz Vino", () => { calizVino.enabled = true; liquidVino.enabled = true; } },
            { "Oil Lamp", () => { oilLamp.enabled = true; flashLightLogic.enabled = true; } },
            { "Ouija", () => { ouija.enabled = true; ouijaLogic.enabled = true; } },
            { "Cross", () => { cross.enabled = true; crossLogic.enabled = true; isProtected = true; playerHealth.protectionLogic = crossLogic; }},
            { "Rosary", () => { rosary.enabled = true; rosaryLogic.enabled = true; isProtected = true; playerHealth.protectionLogic = rosaryLogic; }},
            { "Incienso", () => { incienso.enabled = true; incenseLogic.enabled = true; } },
            { "Key", () => { key.enabled = true; keysLogic.enabled = true; } },
            { "Encendedor", () => { encendedor.enabled = true; encendedorLogic.enabled = true; } },
            { "Cenizas", () => { cenizas.enabled = true; cenizasLogic.enabled = true; } },
            { "Campanillas", () => { campanilla.enabled = true; } },
            { "Cuadro", () => { cuadro.enabled = true; } },
            { "Banderin", () => { banderin.enabled = true; } },
            { "Sal", () => { sal.enabled = true; } },
            { "Palo Santo", () => { paloSanto.enabled = true; paloSantoLogic.enabled = true; } },
            { "AguaBendita", () => { aguaBendita.enabled = true; } },
            { "Caliz Hostia", () => { calizHostia.enabled = true; } }
        };

        dropActions = new Dictionary<string, System.Action>
        {
            { "Oil Lamp", () => { oilLamp.enabled = false; flashLightLogic.enabled = false; if (!pickedObject) flashLightLogic.pointLight.SetActive(false); }},
            { "Ouija", () => { ouija.enabled = false; ouijaLogic.enabled = false; } },
            { "Encendedor", () => { encendedor.enabled = false; encendedorLogic.enabled = false;
                if (!pickedObject)
                {
                    encendedorLogic.pointLight.SetActive(false);
                    encendedorLogic.isLighted = false;
                }
            }},
            { "Cenizas", () => { cenizas.enabled = false; cenizasLogic.enabled = false; } },
            { "Incienso", () => { incienso.enabled = false; incenseLogic.enabled = false;
                if (!pickedObject)
                {
                    incenseLogic.particles.SetActive(false);
                    incenseLogic.isEnabled = false;
                }
            }},
            { "Key", () => { key.enabled = false; keysLogic.enabled = false; } },
            { "Caliz Vino", () => { calizVino.enabled = false; liquidVino.enabled = false; } },
            { "Caliz Hostia", () => { calizHostia.enabled = false; } },
            { "Cross", () => { cross.enabled = false; crossLogic.enabled = false; isProtected = false; }},
            { "Rosary", () => { rosary.enabled = false; rosaryLogic.enabled = false; isProtected = false; }},
            { "Palo Santo", () => { paloSanto.enabled = false; paloSantoLogic.enabled = false; } },
            { "Campanillas", () => { campanilla.enabled = false; } },
            { "Cuadro", () => { cuadro.enabled = false; } },
            { "Banderin", () => { banderin.enabled = false; } },
            { "Sal", () => { sal.enabled = false; } },
            { "AguaBendita", () => { aguaBendita.enabled = false; } }
        };
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