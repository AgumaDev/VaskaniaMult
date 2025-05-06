using System.Collections;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Material dissolveMat;

    private void Start()
    {
        dissolveMat.SetFloat("_DissolveAmount", 0f);
        dissolveMat = GetComponent<MeshRenderer>().materials[0];
    }

    public IEnumerator DissolveCo(float duration)
    {
        float elapsed = 0f; // tiempo acumulado desde que empezó
        float startValue = dissolveMat.GetFloat("_DissolveAmount"); // valor inicial
        float targetValue = 1f; // valor final que queremos alcanzar

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // aumentamos el tiempo según lo que pasa por frame

            // Interpolamos entre el valor inicial y final según cuánto tiempo ha pasado
            float newValue = Mathf.Lerp(startValue, targetValue, elapsed / duration);

            // Asignamos ese nuevo valor al shader
            dissolveMat.SetFloat("_DissolveAmount", newValue);

            // Esperamos al siguiente frame antes de continuar
            yield return null;
        }

        // Aseguramos que el valor final sea exactamente 1 (por si el bucle no llegó exacto)
        dissolveMat.SetFloat("_DissolveAmount", targetValue);
    }
}
