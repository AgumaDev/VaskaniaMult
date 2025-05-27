using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class TestDoTween : MonoBehaviour
{
    public Transform pageToAnim;
    bool isRotated;
    public Ease curveAnimation;

    Light testLight;
    Light2D test2D;
    private void Start()
    {
        DOTween.defaultAutoKill = true;
    }

    private void Update()
    {
        //test2D.do
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //isRotated = !isRotated;
            //if (isRotated)
            //{
            //    //pageToAnim.parent.parent.transform.DOMoveY(0.1f, 1f).SetLoops(-1, LoopType.Yoyo);
            //    pageToAnim.DOLocalRotate(new Vector3(165, -90, -90), 1f, RotateMode.Fast).SetEase(curveAnimation).OnComplete(Finished);
            //}
            //else
            //{
            //    pageToAnim.DOLocalRotate(new Vector3(14.668f, -90, -90), 1f);
            //}

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(pageToAnim.DOLocalRotate(new Vector3(165, -90, -90), 1f, RotateMode.Fast).SetEase(curveAnimation));
            mySequence.AppendInterval(0.5f).OnComplete(() => print("HOLA"));
            mySequence.Append(pageToAnim.DOLocalRotate(new Vector3(14.668f, -90, -90), 1f, RotateMode.Fast).SetEase(curveAnimation));
                mySequence.Play().SetAutoKill(true);
            //mySequence.Elapsed
        }



        //pageToAnim.DOLocalRotate(new Vector3(165, -90, -90), 1f, RotateMode.Fast).SetEase(curveAnimation).OnComplete(Finished);
    }

    void Finished()
    {
        print("SE ACABÓ");
    }
}
