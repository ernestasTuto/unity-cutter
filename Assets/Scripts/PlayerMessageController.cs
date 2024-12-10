using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMessageController : NetworkBehaviour
{
    [SerializeField] private PlayerMessage messagePrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // upon pressing Keyboard.E sends message to server which sends to all clients, sender displays different message
            int _number = Random.Range(0, 100);
            RPC_SendMessage(_number);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(int _number, RpcInfo info = default)
    {
        RPC_RelayMessage(_number, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(int _number, PlayerRef messageSource)
    {
        PlayerMessage _newMessage = Instantiate(messagePrefab, transform);

        bool _isSender = messageSource == Runner.LocalPlayer;
        string _messageText = _isSender ? "I Sent: " + _number : "Hello " + _number;
        _newMessage.SetMessage(_messageText);
    }
}
