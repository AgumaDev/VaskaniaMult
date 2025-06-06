using UnityEngine;
using DG.Tweening;

public class InteractuableIzquierda : MonoBehaviour
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
            transform.DOLocalRotate(new Vector3(0, 0, 90), 0.5f).SetEase(ease);
        }
        else
        {
            
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetEase(ease);
            
        }
    }
}