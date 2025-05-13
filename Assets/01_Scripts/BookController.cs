using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BookController : MonoBehaviour
{
    public int currentBookPage;

    public Animator anim;

    public List<Material> pages = new List<Material>();

    public Renderer bookRenderer;
    public Renderer leftRenderer;
    public Renderer rightRenderer;
    public Renderer midLeftRenderer;
    public Renderer midRightRenderer;

    public bool isActive;

    public bool switchedLeft;
    public bool switchedRight;

    public GameObject pageLeft;
    public GameObject pageRight;
    
    public GameObject pageTransitionLeft;
    public GameObject pageTransitionRight;

    public GameObject izqDer;
    public GameObject derIzq;

    public float changeTimer;

    private void Start()
    {
        isActive = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isActive = !isActive;
        }

        if (isActive)
        {
            bookRenderer.enabled = true;
            leftRenderer.enabled = true;
            rightRenderer.enabled = true;
            midLeftRenderer.enabled = true;
            midRightRenderer.enabled = true;
            
            if (changeTimer < 0)
            {
                changeTimer = 0;
            }

            changeTimer = changeTimer - Time.deltaTime;

            if (currentBookPage != 0 && changeTimer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    switchedLeft = true;
                    switchedRight = false;
                    changeTimer = 0.25f;
                    anim.GetComponent<Animator>().SetTrigger("PageLeft");
                    izqDer.GetComponent<Animator>().SetTrigger("PageLeft");
                    currentBookPage--;
                    currentBookPage--;
                }
            }

            if (currentBookPage != 10 && changeTimer <= 0)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    
                    switchedLeft = false;
                    switchedRight = true;
                    changeTimer = 0.25f;
                    anim.GetComponent<Animator>().SetTrigger("PageRight");
                    izqDer.GetComponent<Animator>().SetTrigger("PageRight");
                    currentBookPage++;
                    currentBookPage++;
                }
            }

            if(changeTimer > 0 && switchedLeft)
            {
                pageLeft.GetComponent<Renderer>().material = pages[currentBookPage];
                pageTransitionLeft.GetComponent<Renderer>().material = pages[currentBookPage+1];
                pageTransitionRight.GetComponent<Renderer>().material = pages[currentBookPage+2];
                pageRight.GetComponent<Renderer>().material = pages[currentBookPage +3];
            }
            else if(changeTimer > 0 && switchedRight)
            {
                pageLeft.GetComponent<Renderer>().material = pages[currentBookPage - 2];
                pageTransitionLeft.GetComponent<Renderer>().material = pages[currentBookPage -1];
                pageTransitionRight.GetComponent<Renderer>().material = pages[currentBookPage];
                pageRight.GetComponent<Renderer>().material = pages[currentBookPage + 1];
            }
            else
            {
                pageLeft.GetComponent<Renderer>().material = pages[currentBookPage];
                pageRight.GetComponent<Renderer>().material = pages[currentBookPage + 1];
                
            }
        }
        else
        {
            bookRenderer.enabled = false;
            leftRenderer.enabled = false;
            rightRenderer.enabled = false;
            midLeftRenderer.enabled = false;
            midRightRenderer.enabled = false;
        }


    }
}
