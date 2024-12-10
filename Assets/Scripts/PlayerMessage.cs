using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    private void Start()
    {
        Invoke("Destroy", 5f);
    }

    public void SetMessage(string _message)
    {
        messageText.text = _message;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
