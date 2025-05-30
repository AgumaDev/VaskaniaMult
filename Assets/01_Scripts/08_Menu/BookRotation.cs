using UnityEngine;
using DG.Tweening;

public class BookRotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool isRotated;
    public Transform rotatingPage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenBook();
        }
    }

    public void OpenBook()
    {
        isRotated = !isRotated;
        if (isRotated)
        {
            rotatingPage.DORotate(new Vector3(-89.2171555f,69.1857758f,270.000427f), 1, RotateMode.Fast);
            rotatingPage.DOMove(new Vector3(-100.300003f, 0, 43.2999992f), 1);
        }
        else
        {
            rotatingPage.DORotate(new Vector3(89.2171555f,69.1857758f,270.000427f), 1, RotateMode.Fast);
            rotatingPage.DOMove(new Vector3(-100.300003f, 3.79999995f, 43.2999992f), 1);
        }
    }
}
