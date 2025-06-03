using UnityEngine;
using DG.Tweening;
using TMPro;

public class ButtonAnimation : MonoBehaviour
{
    public bool isHovered;
    
    
    public void OnHover()
    {
        isHovered = !isHovered;
        if (isHovered)
        {
            GetComponentInChildren<TextMeshProUGUI>().DOColor(Color.white, 0.1f);
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().DOColor(Color.gray, 0.1f);
        }

    }
}