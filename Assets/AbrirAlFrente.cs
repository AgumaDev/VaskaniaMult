using UnityEngine;
using DG.Tweening;

public class AbrirAlFrente : MonoBehaviour
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
            transform.DOLocalMove(new Vector3(transform.localPosition.x - 0.1f, transform.localPosition.y, transform.localPosition.z), 0.5f);
        }
        else
        {
            transform.DOLocalMove(new Vector3(transform.localPosition.x + 0.1f, transform.localPosition.y, transform.localPosition.z), 0.5f);
        }
    }
}