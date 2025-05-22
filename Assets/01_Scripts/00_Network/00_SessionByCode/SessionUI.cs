using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SessionUI : MonoBehaviour
{
    [Header("Player Info")]
    public TMP_InputField playerNameInput;

    [Header("Create Session")]
    public Button createSessionButton;

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
        SessionManager.Instance.SetHostIntent();

        StartCoroutine(LoadLobbyScene());
    }

    void OnJoinSessionClicked()
    {
        var name = playerNameInput.text;
        SessionManager.Instance.PlayerName = string.IsNullOrWhiteSpace(name) ? "Jugador" : name;

        var code = joinCodeInput.text;
        SessionManager.Instance.SetClientIntent(code);

        StartCoroutine(LoadLobbyScene());
    }

    IEnumerator LoadLobbyScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby");
        while (!asyncLoad.isDone)
            yield return null;
    }
}