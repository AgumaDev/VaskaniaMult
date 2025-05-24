using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void TryStartGame()
    {
        if (!NetworkManager.Singleton.IsHost)
            return;

        NetworkManager.Singleton.SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}