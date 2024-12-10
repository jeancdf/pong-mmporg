using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public UDPService UDP;
    public int ListenPort = 25000;

    private Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
    private Dictionary<IPEndPoint, string> clientPaddleAssignments = new Dictionary<IPEndPoint, string>();

    void Awake()
    {
        if (!Globals.IsServer)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        UDP.Listen(ListenPort);

        UDP.OnMessageReceived += (string message, IPEndPoint sender) =>
        {
            Debug.Log($"Message : {message}");
            if (message.StartsWith("CONNECT"))
            {
                HandleNewConnection(sender);
            }
            else if (message.StartsWith("UPDATE|PADDLE|"))
            {
                Debug.Log("[SERVER] Message received from " +
                          sender.Address.ToString() + ":" + sender.Port
                          + " => " + message);
                HandlePaddleUpdate(message, sender);
            }
        };
    }

    private void HandleNewConnection(IPEndPoint sender)
    {
        string addr = sender.Address.ToString() + ":" + sender.Port;
        if (!clients.ContainsKey(addr))
        {
            clients.Add(addr, sender);
            string assignedPaddle = AssignPaddleToClient(sender);
            
            Debug.Log($"Assigned paddle {assignedPaddle} to client {addr}");
            
            SendPaddleAssignment(sender, assignedPaddle);
        }
        Debug.Log("There are " + clients.Count + " clients present.");
    }

    private string AssignPaddleToClient(IPEndPoint clientEndPoint)
    {
        string assignedPaddle = "PaddleLeft";
        if (clientPaddleAssignments.ContainsValue("PaddleLeft"))
        {
            assignedPaddle = "PaddleRight";
        }
        
        clientPaddleAssignments[clientEndPoint] = assignedPaddle;
        return assignedPaddle;
    }

    private void SendPaddleAssignment(IPEndPoint clientEndPoint, string paddle)
    {
        string message = $"ASSIGN|PADDLE|{paddle}";
        UDP.SendUDPMessage(message, clientEndPoint);
    }

    public void BroadcastUDPMessage(string message, IPEndPoint excludeClient = null)
    {
        foreach (KeyValuePair<string, IPEndPoint> client in clients)
        {
            if (client.Value.Equals(excludeClient)) continue;
            UDP.SendUDPMessage(message, client.Value);
        }
    }
    
    private void HandlePaddleUpdate(string message, IPEndPoint sender)
    {

            // Découper le message pour extraire les données
            string[] tokens = message.Split('|');
            if (tokens.Length < 4) return;

            string paddleSide = tokens[2]; // LEFT ou RIGHT
            string positionData = tokens[3]; // Y:<position>

            // Extraire la position Y
            if (!positionData.StartsWith("Y:")) return;
            float newPositionY = float.Parse(positionData.Substring(2), System.Globalization.CultureInfo.InvariantCulture);

            // Trouver le paddle correspondant
            foreach (KeyValuePair<IPEndPoint, string> assignment in clientPaddleAssignments)
            {
                if (assignment.Key.Equals(sender) && assignment.Value == paddleSide)
                {
                    // Appliquer la nouvelle position au paddle
                    GameObject paddle = GameObject.Find(paddleSide); // Assurez-vous que les paddles ont des noms correspondants
                    if (paddle != null)
                    {
                        Vector3 currentPosition = paddle.transform.position;
                        paddle.transform.position = new Vector3(currentPosition.x, newPositionY, currentPosition.z);
                    }
                    break;
                }
            }
        
    }
}
