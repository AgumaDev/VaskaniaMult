using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Vivox;
using System.Threading.Tasks;

public class VoiceChatManager : MonoBehaviour
{
    public static VoiceChatManager instance;

    [SerializeField] private string userName;
    private GameObject localPlayer;
    private bool isInChannel = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isInChannel && localPlayer != null)
        {
            VivoxService.Instance.Set3DPosition(localPlayer, SessionManager.instance.activeSession.Id.ToString());
        }
    }

    public async Task InitializeAsync()
    {
        await UnityServices.InitializeAsync();
        await VivoxService.Instance.InitializeAsync();
        await LoginToVivoxAsync();
    }

    public async Task LoginToVivoxAsync()
    {
        userName = SessionManager.instance.PlayerName;

        var options = new LoginOptions
        {
            DisplayName = userName,
            EnableTTS = false
        };

        Debug.Log($"{options.DisplayName} is logged in!");
        await VivoxService.Instance.LoginAsync(options);
    }

    public async Task JoinVoiceChannelAsync()
    {
        if (SessionManager.instance.activeSession != null)
        {
            string channelId = SessionManager.instance.activeSession.Id;

            await VivoxService.Instance.JoinPositionalChannelAsync(
                channelId,
                ChatCapability.AudioOnly,
                new Channel3DProperties(30, 10, 2, AudioFadeModel.ExponentialByDistance)
            );

            isInChannel = true;
            Debug.Log("Se unió correctamente al canal de voz: " + channelId);
        }
        else
        {
            Debug.LogError("No se puedo unir al canal de voz: activeSession es null");
        }
    }

    public void SetLocalPlayer(GameObject player)
    {
        if (localPlayer == null)
            localPlayer = player;
    }
}