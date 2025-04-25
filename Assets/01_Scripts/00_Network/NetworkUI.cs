using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class NetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }
    void HostButtonOnClick()
    {
        NetworkManager.Singleton.StartHost();
    }
    void ClientButtonOnClick()
    {
        NetworkManager.Singleton.StartClient();
    }
}
