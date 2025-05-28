using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class VelaCounter : NetworkBehaviour
{
    public static VelaCounter instance;
    public List<GameObject> velas;

    public int lightenedUp;
    private void Awake()
    {
        if (instance != null) Destroy(this); else instance = this;
    }

    [Rpc(SendTo.Everyone)]
    public void VelaCountRpc()
    {
        lightenedUp = 0;
        
        for (int i = 0; i < velas.Count; i++)
        {
            if (velas[i].GetComponent<VelaPisoLogic>().isLightenUp)
                lightenedUp++;
        }
        
        if (lightenedUp == 5)
        {
            ItemRecognitionArea.instance.candlesActivated = true; 
        }
    }

}
