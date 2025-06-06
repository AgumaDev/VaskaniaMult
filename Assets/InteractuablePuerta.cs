using UnityEngine;
using DG.Tweening;

public class InteractuablePuerta : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool isOpened;
    public Ease ease;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        isOpened = !isOpened;
        if (isOpened)
        {
            transform.DOLocalRotate(new Vector3(-90, -90, transform.localRotation.z), 1f).SetEase(ease);
        }
        else
        {
            transform.DOLocalRotate(new Vector3(-90, 0, transform.localRotation.z), 1f).SetEase(ease);
        }
    }
}