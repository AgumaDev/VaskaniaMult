using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using System.Threading.Tasks;

public class VoiceChatManager : MonoBehaviour
{
    #region REFERENCES
    #endregion

    #region VARIABLES
    public string userName;
    #endregion

    static VoiceChatManager instance;

    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAsync();
    }

    async void InitializeAsync()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await VivoxService.Instance.InitializeAsync();
        await LoginToVivoxAsync();
    }

    public async Task LoginToVivoxAsync()
    {
        LoginOptions options = new LoginOptions();
        options.DisplayName = $"{userName}{Random.Range(0, 1000)}";
        options.EnableTTS = true;
        await VivoxService.Instance.LoginAsync(options);
        //await JoinEchoChannelAsync();
        await JoinGroupChannelAsync();
    }

    public async Task JoinPositionalGroupChannelAsync()
    {
        string channelToJoin = SessionManager.instance.activeSession.Id.ToString();
        await VivoxService.Instance.JoinPositionalChannelAsync(channelToJoin, ChatCapability.AudioOnly, new Channel3DProperties(2, 2, 4f, AudioFadeModel.ExponentialByDistance));
    }

    public async Task JoinEchoChannelAsync()
    {
        string channelToJoin = "Lobby";
        await VivoxService.Instance.JoinEchoChannelAsync(channelToJoin, ChatCapability.TextAndAudio);
    }

    public async Task JoinGroupChannelAsync()
    {
        string channelToJoin = "MultiLobby";
        await VivoxService.Instance.JoinGroupChannelAsync(channelToJoin, ChatCapability.AudioOnly);
    }

}


