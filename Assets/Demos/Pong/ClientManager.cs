using System.Net;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public UDPSender Sender;

    private float NextCoucouTimeout = -1;

    void Awake() {
        // Desactiver mon objet si je ne suis pas le client
        if (Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sender.OnMessageReceived = (string message, IPEndPoint sender) => {
            Debug.Log("[CLIENT] Message received from " + 
                sender.Address.ToString() + ":" + sender.Port 
                + " =>" + message);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > NextCoucouTimeout) {
            Sender.SendUDPMessage("coucou");
            NextCoucouTimeout = Time.time + 0.5f;
        }
    }
}
