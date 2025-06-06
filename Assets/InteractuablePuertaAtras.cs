using UnityEngine;
using DG.Tweening;

public class InteractuablePuertaAtras : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Ease ease;
    public GameObject puerta;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        GetComponentInParent<InteractuablePuerta>().isOpened = GetComponentInParent<InteractuablePuerta>().isOpened;
        if (GetComponentInParent<InteractuablePuerta>().isOpened)
        {
            puerta.transform.DOLocalRotate(new Vector3(-90, 0, transform.localRotation.z), 1f).SetEase(ease);
            GetComponentInParent<InteractuablePuerta>().isOpened = false;
        }
        else if(GetComponentInParent<InteractuablePuerta>().isOpened == false)
        {
            puerta.transform.DOLocalRotate(new Vector3(-90, 90, transform.localRotation.z), 1f).SetEase(ease);
            GetComponentInParent<InteractuablePuerta>().isOpened = true;
        }
    }
}