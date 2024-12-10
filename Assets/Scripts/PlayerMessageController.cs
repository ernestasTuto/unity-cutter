using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class PlayerMessageController : NetworkBehaviour
{
    [SerializeField] private PlayerMessage messagePrefab;

    private void Update()
    {
        // on input asks servers to send message to everyone, sender receives different message
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendMessageServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendMessageServerRpc(ulong _senderID)
    {
        int _number = UnityEngine.Random.Range(0, 100);
        SendMessageClientRpc(_senderID, _number);
    }

    [ClientRpc]
    private void SendMessageClientRpc(ulong _senderID, int _number)
    {
        PlayerMessage _newMessage = Instantiate(messagePrefab, transform);

        bool _isSender = _senderID == NetworkManager.Singleton.LocalClientId; 
        string _messageText = _isSender ? "I Sent: " + _number : "Hello " + _number;
        _newMessage.SetMessage(_messageText);
    }
}
