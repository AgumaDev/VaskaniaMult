using UnityEngine;
public class ItemRecognitionArea : MonoBehaviour
{
    private static ItemRecognitionArea _instance;
    public static ItemRecognitionArea Instance { get { return _instance; } }

    private int coreItemNumber;
    public int coreItemActivated;

    public bool candleActivated;
    
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
        if (candleActivated == true)
        {
            coreItemActivated++;
            candleActivated = false;
        }
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
