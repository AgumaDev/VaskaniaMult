using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionUI : MonoBehaviour
{
    [Header("Player Info")]
    public TMP_InputField playerNameInput;

    [Header("Create Session")]
    public Button createSessionButton;
    public TMP_Text sessionCodeText;

    [Header("Join Session")]
    public TMP_InputField joinCodeInput;
    public Button joinSessionButton;
    void Start()
    {
        createSessionButton.onClick.AddListener(OnCreateSessionClicked);
        joinSessionButton.onClick.AddListener(OnJoinSessionClicked);
    }
    void OnCreateSessionClicked()
    {
        var name = playerNameInput.text;
        SessionManager.Instance.PlayerName = string.IsNullOrWhiteSpace(name) ? "Jugador" : name;
        SessionManager.Instance.CreateSession((code) => {
            sessionCodeText.text = $"Código: {code}";
        });
    }
    void OnJoinSessionClicked()
    {
        var name = playerNameInput.text;
        SessionManager.Instance.PlayerName = string.IsNullOrWhiteSpace(name) ? "Jugador" : name;
        var code = joinCodeInput.text;
        SessionManager.Instance.JoinSession(code);
    }
}
