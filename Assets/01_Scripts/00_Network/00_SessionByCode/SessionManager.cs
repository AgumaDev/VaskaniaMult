using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityUtils;

public class SessionManager : Singleton<SessionManager>
{
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
    public string PlayerId => AuthenticationService.Instance.PlayerId;
    
    public Action<Dictionary<string, string>, string, bool> OnPlayerListUpdated;

    async void Start()
    {
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
    public async void CreateSession(Action<string> onSessionCreated)
    {
        var playerProperties = new Dictionary<string, PlayerProperty>
        {
            { playerNamePropertyKey, new PlayerProperty(PlayerName, VisibilityPropertyOptions.Member) }
        };

        var options = new SessionOptions
        {
            MaxPlayers = 4,
            IsLocked = false,
            IsPrivate = false,
            PlayerProperties = playerProperties
        }.WithRelayNetwork();

        ActiveSession = await MultiplayerService.Instance.CreateSessionAsync(options);
        Debug.Log($"Session {ActiveSession.Id} created! Join code: {ActiveSession.Code}");
        onSessionCreated?.Invoke(ActiveSession.Code);

        MonitorSessionPlayers();
    }
    public async void JoinSession(string sessionCode)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(PlayerName);

        ActiveSession = await MultiplayerService.Instance.JoinSessionByCodeAsync(sessionCode);
        Debug.Log($"Joined session {ActiveSession.Id}");

        MonitorSessionPlayers();
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

    public async void LeaveSession()
    {
        if (ActiveSession != null)
        {
            try
            {
                await ActiveSession.AsHost().LeaveAsync();
            }
            catch
            {
                // Ignored
            }
            finally
            {
                ActiveSession = null;
            }
        }
    }
}
