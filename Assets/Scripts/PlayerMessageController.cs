using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessageController : NetworkBehaviour
{
    [SerializeField] private PlayerMessage messagePrefab;

    private void Update()
    {
        // on input asks servers to send message to everyone, sender receives different message
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendMessageServerRpc(base.LocalConnection);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendMessageServerRpc(NetworkConnection _target)
    {
        int _number = UnityEngine.Random.Range(0, 100);
        SendMessageObserversRpc(_target, _number);
    }

    [ObserversRpc]
    private void SendMessageObserversRpc(NetworkConnection _target, int _number)
    {
        PlayerMessage _newMessage = Instantiate(messagePrefab, transform);

        bool _isSender = _target == base.LocalConnection;
        string _messageText = _isSender ? "I Sent: " + _number : "Hello " + _number;
        _newMessage.SetMessage(_messageText);
    }
}
