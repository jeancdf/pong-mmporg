using System.Net;
using UnityEngine;
using TMPro; // Pour TextMeshPro

public class ClientManager : MonoBehaviour
{
    public UDPService UDP;
    public string ServerIP = Globals.IPAddress;
    public int ServerPort = 25000;

    public TMP_InputField ipInputField;

    private float NextCoucouTimeout = -1;
    private IPEndPoint ServerEndpoint;

    

    void Awake() {
        // Desactiver mon objet si je ne suis pas le client
        if (Globals.IsServer) {
            gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        UDP.InitClient();

        ServerEndpoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
            
        UDP.OnMessageReceived += (string message, IPEndPoint sender) => {
            Debug.Log("[CLIENT] Message received from " + 
                sender.Address.ToString() + ":" + sender.Port 
                + " =>" + message);
            if (message.StartsWith("Role:"))
    {
                GetRole(message);
    }
        };
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Time.time > NextCoucouTimeout) {
            UDP.SendUDPMessage("coucou", ServerEndpoint);
            NextCoucouTimeout = Time.time + 0.5f;
        }
    }


    // Méthode pour mettre à jour l'adresse IP
    public void UpdateServerIP(string newIP)
    {
        if (IPAddress.TryParse(newIP, out IPAddress parsedIP))
        {
            ServerIP = newIP;
            UpdateServerEndpoint(); // Redéfinir ServerEndpoint
            Debug.Log("Nouvelle adresse IP du serveur définie : " + ServerIP);
        }
        else
        {
            Debug.LogWarning("Adresse IP invalide : " + newIP);
        }
    }

    // Méthode pour mettre à jour ServerEndpoint
    private void UpdateServerEndpoint()
    {
        ServerEndpoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
        Debug.Log("ServerEndpoint mis à jour : " + ServerEndpoint);
    }

    public void GetRole(string message){
        if (message.StartsWith("Role:"))
    {
        string role = message.Split(':')[1];
        Debug.Log($"Rôle reçu du serveur : {role}");

        // Attribuez le rôle à chaque paddle
        foreach (var paddle in FindObjectsOfType<PaddleController>())
        {
            Globals.Player = role;
            paddle.OnRoleAssigned(role);
        }
        Debug.Log(Globals.Player);
    }
    }
}
