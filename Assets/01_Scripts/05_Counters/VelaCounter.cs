using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class VelaCounter : NetworkBehaviour
{
    public static VelaCounter _instance;
    public List<GameObject> velas = new List<GameObject>();

    public int lightenedUp;
    private void Awake()
    {
        if (_instance != null) Destroy(this); else _instance = this;
    }
    private void Start()
    {
        if (!IsServer)
            return;
        
        velas.AddRange(GameObject.FindGameObjectsWithTag("Vela"));
    }

    [Rpc(SendTo.Everyone)]
    public void VelaCountRpc()
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
            ItemRecognitionArea._instance.candleActivated = true;
        }
    }

}
