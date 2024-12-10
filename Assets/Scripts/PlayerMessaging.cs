using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessaging : NetworkBehaviour
{
    [Client]
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // on input asks servers to send message to everyone, sender receives different message
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendMessage();
        }
    }

    private void SendMessage()
    {
        int _number = Random.Range(0, 100);
        PlayerMessageController.instance.LocalMessage(_number);
        SendMessageToOthersServerRpc(_number);
    }

    [Command(requiresAuthority = false)]
    private void SendMessageToOthersServerRpc(int _number)
    {
        SendMessageToOthersClientRpc(_number);
    }

    [ClientRpc(includeOwner = false)]
    private void SendMessageToOthersClientRpc(int _number)
    {
        PlayerMessageController.instance.MessageReceived(_number);
    }
}
