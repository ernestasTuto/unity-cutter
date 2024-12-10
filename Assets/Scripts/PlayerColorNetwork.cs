using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Demo.AdditiveScenes;

// changes player color and synchronizes using RPC when Keyboard.F is pressed

public class PlayerColorNetwork : NetworkBehaviour
{
    public GameObject body;

    private Renderer rend;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<PlayerColorNetwork>().enabled = false;    
    }

    private void Start()
    {
        rend = gameObject.GetComponent<PlayerColorNetwork>().body.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (rend.material.color == Color.cyan)
            {
                ChangeColorServer(Color.magenta);
            }
            else
            {
                ChangeColorServer(Color.cyan);
            }      
        }
    }

    [ServerRpc]
    public void ChangeColorServer(Color color)
    {
        ChangeColor(color);
    }

    [ObserversRpc]
    public void ChangeColor(Color color)
    {
        rend.material.color = color;
    }
}