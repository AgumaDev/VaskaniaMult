using UnityEngine;

public class InteractuableRaycast : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject cam;
    public RaycastHit hit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.magenta);
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("a");
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000);
            if (hit.collider.transform.GetComponent<InteractuableDerecha>())
            {
                hit.collider.GetComponent<InteractuableDerecha>().Open();
                print("b");
            }

            if (hit.collider.transform.GetComponent<InteractuableIzquierda>())
            {
                hit.collider.GetComponent<InteractuableIzquierda>().Open();
            }
            if (hit.collider.transform.GetComponent<InteractuablePuerta>())
            {
                hit.collider.GetComponent<InteractuablePuerta>().Open();
            }
            if (hit.collider.transform.GetComponent<InteractuablePuertaAtras>())
            {
                hit.collider.GetComponent<InteractuablePuertaAtras>().Open();
            }

            if (hit.collider.transform.GetComponent<AbrirAlFrente>())
            {
                hit.collider.GetComponent<AbrirAlFrente>().Open();
            }
        }
    }
}
