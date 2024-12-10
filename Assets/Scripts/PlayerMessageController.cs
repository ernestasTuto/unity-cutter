using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// class that displays messages

public class PlayerMessageController : MonoBehaviour
{
    public static PlayerMessageController instance;
    [SerializeField] private PlayerMessage messagePrefab;

    private void Start()
    {
        instance = this;
    }

    public void LocalMessage(int _number)
    {
        PlayerMessage _newMessage = Instantiate(messagePrefab, transform);
        string _messageText = "I Sent: " + _number;
        _newMessage.SetMessage(_messageText);
    }

    public void MessageReceived(int _number)
    {
        PlayerMessage _newMessage = Instantiate(messagePrefab, transform);
        string _messageText = "Hello " + _number;
        _newMessage.SetMessage(_messageText);
    }
}
