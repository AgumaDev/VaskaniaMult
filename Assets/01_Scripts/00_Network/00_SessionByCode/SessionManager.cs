using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;

public enum SessionIntent
{
    None,
    Host,
    Client
}

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    ISession activeSession;
    ISession ActiveSession
    {
        get => activeSession;
        set
        {
            activeSession = value;
            Debug.Log($"Active session: {activeSession}");
        }
    }

    const string playerNamePropertyKey = "playerName";
    public string PlayerName { get; set; } = "Jugador";

    public string CurrentSessionCode { get; private set; }
    public string PlayerId => AuthenticationService.Instance.PlayerId;

    public SessionIntent PendingIntent { get; private set; } = SessionIntent.None;
    public string PendingJoinCode { get; private set; }

    public Action<Dictionary<string, string>, string, bool> OnPlayerListUpdated;

    async void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void SetHostIntent() => PendingIntent = SessionIntent.Host;
    public void SetClientIntent(string joinCode)
    {
        PendingIntent = SessionIntent.Client;
        PendingJoinCode = joinCode;
    }

    public async Task<bool> CreateSession()
    {
        try
        {
            var playerProperties = new Dictionary<string, PlayerProperty>
            {
                { playerNamePropertyKey, new PlayerProperty(PlayerName, VisibilityPropertyOptions.Member) }
            };

            var options = new SessionOptions
            {
                MaxPlayers = 5,
                IsLocked = false,
                IsPrivate = false,
                PlayerProperties = playerProperties
            }.WithRelayNetwork();

            ActiveSession = await MultiplayerService.Instance.CreateSessionAsync(options);
            Debug.Log($"Session {ActiveSession.Id} created! Join code: {ActiveSession.Code}");

            CurrentSessionCode = ActiveSession.Code;

            MonitorSessionPlayers();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create session: {e}");
            return false;
        }
    }

    public async Task<bool> JoinSession(string sessionCode)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(PlayerName);

            ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionCode);
            Debug.Log($"Joined session {ActiveSession.Id}");

            CurrentSessionCode = ActiveSession.Code;
            MonitorSessionPlayers();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to join session: {e}");
            return false;
        }
    }

    async void MonitorSessionPlayers()
    {
        while (ActiveSession != null)
        {
            var players = ActiveSession.Players;
            var dict = new Dictionary<string, string>();

            foreach (var p in players)
            {
                if (p.Properties.TryGetValue(playerNamePropertyKey, out var prop))
                    dict[p.Id] = prop.Value.ToString();
                else
                    dict[p.Id] = "Desconocido";
            }

            OnPlayerListUpdated?.Invoke(dict, PlayerId, ActiveSession.IsHost);
            await Task.Delay(1000);
        }
    }
}
