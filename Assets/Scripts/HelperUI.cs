using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// basic class for simple connecting ui methods

public class HelperUI : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
        {
            networkManager.StartHost();
        }
        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
        {
            networkManager.StartClient();
        }
    }
}
