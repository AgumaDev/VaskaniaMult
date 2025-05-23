using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRecognitionArea : MonoBehaviour
{
    private static ItemRecognitionArea _instance;
    public static ItemRecognitionArea Instance { get { return _instance; } }

    private int coreItemNumber;
    public int coreItemActivated;
    
    public TextMeshProUGUI coreItemText;

    public bool candleActivated;

    public GameObject nunEvent;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    void Update()
    {
        coreItemText.text = coreItemActivated.ToString();
        
        if (candleActivated == true)
        {
            coreItemActivated++;
            candleActivated = false;
        }
        
        if(Input.GetKeyDown(KeyCode.P))
            SpawnNun();

        if (coreItemActivated == 3)
        {
            SpawnNun();
        }
    }

    public void SpawnNun()
    {
        nunEvent.SetActive(true);

    }
    public void DecalCounterUp()
    {
        coreItemActivated++;
    }
    public void DecalCounterDown()
    {
        coreItemActivated--;
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
