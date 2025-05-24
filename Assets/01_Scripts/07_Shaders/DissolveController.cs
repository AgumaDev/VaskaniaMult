using System.Collections;
using UnityEngine.VFX;
using UnityEngine;
public class DissolveController : MonoBehaviour
{
    public Material[] dissolveMat;
    public VisualEffect dissolveVFXGraph;

    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private void Start()
    {
        dissolveMat = GetComponent<MeshRenderer>().materials;
    }
    public IEnumerator DissolveCo()
    {
        if (dissolveMat.Length > 0)
        {
            float counter = 0f;

            while (dissolveMat[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < dissolveMat.Length; i++)
                {
                    dissolveMat[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
        if (dissolveVFXGraph != null)
            dissolveVFXGraph.Play();
    }
}