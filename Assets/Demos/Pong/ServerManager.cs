using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public UDPReceiver Receiver;

    public Dictionary<string, IPEndPoint> Clients = new Dictionary<string, IPEndPoint>(); 

    void Awake() {
        // Desactiver mon objet si je ne suis pas le serveur
        if (!Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        Receiver.Listen(
            (string message, IPEndPoint sender) => {
                Debug.Log("[SERVER] Message received from " + 
                    sender.Address.ToString() + ":" + sender.Port 
                    + " =>" + message);
                
                switch (message) {
                    case "coucou":
                        // Ajouter le client à mon dictionnaire
                        string addr = sender.Address.ToString() + ":" + sender.Port;
                        if (!Clients.ContainsKey(addr)) {
                            Clients.Add(addr, sender);
                        }
                        Debug.Log("There are " + Clients.Count + " clients present.");

                        Receiver.SendUDPMessage("welcome!", sender);
                        break;
                }
                
                //@todo : do something with the message that has arrived! 
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
