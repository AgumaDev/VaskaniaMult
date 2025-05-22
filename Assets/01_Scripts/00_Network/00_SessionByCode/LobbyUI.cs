using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    public TextMeshProUGUI sessionCodeText;
    private bool sessionCodeShown = false;

    void Update()
    {
        if (!sessionCodeShown && !string.IsNullOrEmpty(SessionManager.Instance?.CurrentSessionCode))
        {
            sessionCodeText.text = $"Código: {SessionManager.Instance.CurrentSessionCode}";
            sessionCodeShown = true;
        }
    }
}