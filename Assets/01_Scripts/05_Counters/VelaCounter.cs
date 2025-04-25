using System.Collections.Generic;
using UnityEngine;
public class VelaCounter : MonoBehaviour
{
    private static VelaCounter _instance;
    public static VelaCounter Instance { get { return _instance; } }
    
    public List<GameObject> velas = new List<GameObject>();

    public int lightenedUp;
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else 
            _instance = this;
    }
    private void Start()
    {
        velas.AddRange(GameObject.FindGameObjectsWithTag("Vela"));
    }
    public void VelaCount()
    {
        lightenedUp = 0;
        
        for (int i = 0; i < velas.Count; i++)
        {
            if (velas[i].GetComponent<VelaPisoLogic>().isLightenUp)
            {
                lightenedUp++;
            }
        }
        
        if (lightenedUp == 5)
        {
            ItemRecognitionArea.Instance.candleActivated = true;
        }
    }

}
