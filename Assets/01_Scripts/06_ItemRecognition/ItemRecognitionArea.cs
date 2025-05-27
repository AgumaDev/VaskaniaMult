using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ItemRecognitionArea : NetworkBehaviour
{
    public static ItemRecognitionArea _instance;

    private int coreItemNumber;
    public NetworkVariable<int> coreItemActivated = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    
    public TextMeshProUGUI coreItemText;

    public bool candleActivated;

    public GameObject nunEvent;
    
    private void Awake()
    {
        if (_instance != null) Destroy(this); else _instance = this;
    }
    void Update()
    {
        if(!IsServer)
            return;
        
        coreItemText.text = coreItemActivated.Value.ToString();
        
        if (candleActivated)
        {
            coreItemActivated.Value++;
            candleActivated = false;
        }

        if (coreItemActivated.Value == 3)
        {
            SpawnNunRpc();
        }
    }
    [Rpc(SendTo.Everyone)]
    public void SpawnNunRpc()
    {
        nunEvent.SetActive(true);
    }
    public void DecalCounterUp()
    {
        if(!IsServer)
            return;
        
        coreItemActivated.Value++;
    }
    public void DecalCounterDown()
    {
        if(!IsServer)
            return;
        
        coreItemActivated.Value--;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CoreItem>())
        {
            coreItemNumber++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CoreItem>())
        {
            coreItemNumber--;
        }
    }
}
