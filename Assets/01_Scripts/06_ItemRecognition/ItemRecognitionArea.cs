using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ItemRecognitionArea : NetworkBehaviour
{
    public static ItemRecognitionArea instance;

    private int coreItemNumber;
    public NetworkVariable<int> coreItemActivated;
    public TextMeshProUGUI coreItemText;

    public bool candlesActivated = false;
    private bool candlesHandled = false; // para evitar doble ejecución

    public GameObject nunEvent;

    private void Awake()
    {
        if (instance != null) Destroy(this); else instance = this;
    }

    void Update()
    {
        coreItemText.text = coreItemActivated.Value.ToString();

        if (!IsServer)
            return;

        if (candlesActivated && !candlesHandled)
        {
            CandlesActivated();
            candlesHandled = true;
        }

        if (coreItemActivated.Value == 3)
            SpawnNunRpc();
    }

    [Rpc(SendTo.Everyone)]
    public void SpawnNunRpc()
    {
        nunEvent.SetActive(true);
    }

    public void CandlesActivated()
    {
        Debug.Log("Se activaron todas las velas");
        coreItemActivated.Value++;
        candlesActivated = false;
    }

    public void ResetCandlesFlag()
    {
        candlesHandled = false;
    }

    public void DecalCounterUp()
    {
        if (!IsServer)
            return;

        coreItemActivated.Value++;
    }

    public void DecalCounterDown()
    {
        if (!IsServer)
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