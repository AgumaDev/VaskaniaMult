using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Netcode;

public class PlayerNameDisplay : NetworkBehaviour
{
    public TextMeshProUGUI nameText;

    private readonly NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(
        writePerm: NetworkVariableWritePermission.Owner
    );

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Solo el dueño del objeto establece su nombre
            playerName.Value = SessionManager.Instance.PlayerName;
        }

        playerName.OnValueChanged += (_, newValue) => { nameText.text = newValue.ToString(); };

        // Mostrar el valor actual (incluso si ya está seteado antes de que este cliente se conecte)
        nameText.text = playerName.Value.ToString();
    }

    public override void OnDestroy()
    {
        playerName.OnValueChanged -= (_, __) => { }; // Limpieza
    }
}