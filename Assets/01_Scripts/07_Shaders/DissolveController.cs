using System.Collections;
using UnityEngine.VFX;
using UnityEngine;
public class DissolveController : MonoBehaviour
{
    public Material dissolveMat;
    public VisualEffect dissolveVFXGraph;
    private void Start()
    {
        dissolveMat = GetComponent<MeshRenderer>().materials[0];
        dissolveMat.SetFloat("_DissolveAmount", 0f);
    }
    public IEnumerator DissolveCo(float duration)
    {
        float elapsed = 0f;
        float startValue = dissolveMat.GetFloat("_DissolveAmount");
        float targetValue = 1f;

        if (dissolveVFXGraph != null)
            dissolveVFXGraph.Play();
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // aumentamos el tiempo seg�n lo que pasa por frame

            // Interpolamos entre el valor inicial y final seg�n cu�nto tiempo ha pasado
            float newValue = Mathf.Lerp(startValue, targetValue, elapsed / duration);

            // Asignamos ese nuevo valor al shader
            dissolveMat.SetFloat("_DissolveAmount", newValue);
            
            // Esperamos al siguiente frame antes de continuar
            yield return null;
        }
            // Aseguramos que el valor final sea exactamente 1 (por si el bucle no lleg� exacto)
            dissolveMat.SetFloat("_DissolveAmount", targetValue);   
    }
}