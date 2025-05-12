using UnityEngine;
public class WobbleInHandController : MonoBehaviour
{
    Renderer rend;
    Vector3 lastPos;
    public float maxWobble = 0.03f;
    public float wobbleSpeed = 2f;
    public float recovery = 2f;

    float wobbleX, wobbleZ;
    float time;

    void Start()
    {
        rend = GetComponent<Renderer>();
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 velocity = (transform.position - lastPos) / Time.deltaTime;
        float speed = velocity.magnitude;

        // Si se está moviendo, añade energía al wobble
        if (speed > 0.01f)
        {
            // Añade una cantidad proporcional al movimiento en X y Z
            wobbleX += Mathf.Clamp(velocity.x * maxWobble, -maxWobble, maxWobble);
            wobbleZ += Mathf.Clamp(velocity.z * maxWobble, -maxWobble, maxWobble);
        }

        // Suaviza el wobble con el tiempo (como la inercia del líquido)
        wobbleX = Mathf.Lerp(wobbleX, 0, Time.deltaTime * recovery);
        wobbleZ = Mathf.Lerp(wobbleZ, 0, Time.deltaTime * recovery);

        // Oscilación senoidal con el tiempo
        time += Time.deltaTime;
        float wobbleSinX = wobbleX * Mathf.Sin(time * wobbleSpeed * Mathf.PI * 2);
        float wobbleSinZ = wobbleZ * Mathf.Sin(time * wobbleSpeed * Mathf.PI * 2);

        // Enviar al shader
        rend.material.SetFloat("_WobbleX", wobbleSinX);
        rend.material.SetFloat("_WobbleZ", wobbleSinZ);

        lastPos = transform.position;
    }
}
