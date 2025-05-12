using UnityEngine;
using TMPro;

public class TriggerBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Material mat;
    public bool isHovered;

    public float scaleFactor = 1f;
    public Shader shader;

    public float hoverTime;
    public TextMeshProUGUI text;

    public bool hasBeenClicked;
    
    
    public GameObject cinemachineCamMain;
    public GameObject cinemachineCamBook;
    void Start()
    {
        scaleFactor = 1f;
        mat.SetFloat("_Scale", 1);
        text.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (hasBeenClicked)
        {
            if (scaleFactor > 1)
                scaleFactor -= Time.deltaTime / 100f;
            
            if(text.alpha > 0)
                text.alpha -= Time.deltaTime * 2;
        }
        else
        {
            MatManager();
            hoverTime -= Time.deltaTime;

            if (hoverTime <= 0)
            {
                isHovered = false;
            }

        }
        mat.SetFloat("_Scale", scaleFactor);

    }

    
    
    void MatManager()
    {
        if (isHovered)
        {
            if (scaleFactor < 1.01f)
                scaleFactor += Time.deltaTime / 100f;
            
            if(text.alpha < 1)
            text.alpha += Time.deltaTime * 2;

        }
        else
        {
            if (scaleFactor > 1)
            scaleFactor -= Time.deltaTime / 100f;
            
            if(text.alpha > 0)
            text.alpha -= Time.deltaTime * 2;
            
        }
    }
}
