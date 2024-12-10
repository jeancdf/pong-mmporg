using System.Net;
using UnityEngine;
using TMPro;

public class ClientManager : MonoBehaviour
{
    public UDPService UDP;
    public string ServerIP = Globals.IPAddress;

    public TMP_InputField ipInputField;

    public int ServerPort = 25000;

    public IPEndPoint ServerEndpoint;

    private string assignedPaddle = "";

    public PongPaddle paddleLeftPlayer;
    public PongPaddle paddleRightPlayer;

    void Awake()
    {
        if (Globals.IsServer)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        UDP.InitClient();
    
        ServerEndpoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
    
        SendConnectMessage();

        UDP.OnMessageReceived += (string message, IPEndPoint sender) =>
        {
            if (message.StartsWith("ASSIGN|PADDLE|"))
            {
                HandlePaddleAssignment(message);
            }
            else if (message.StartsWith("UPDATE|PADDLE|"))
            {
                Debug.Log("[CLIENT] Message received from " +
                          sender.Address.ToString() + ":" + sender.Port
                          + " => " + message);
            }
        };
    }

    private void SendConnectMessage()
    {
        UDP.SendUDPMessage("CONNECT", ServerEndpoint);
    }

    private void HandlePaddleAssignment(string message)
    {
        string[] tokens = message.Split('|');
        if (tokens.Length == 3)
        {
            assignedPaddle = tokens[2];

            if (assignedPaddle == "PaddleRight")
            {
                Debug.Log("Disabling paddleRightPlayer, Enabling paddleLeftPlayer");
                paddleLeftPlayer.enabled = true;
                paddleRightPlayer.enabled = false;
            }
            else
            {
                Debug.Log("Disabling paddleLeftPlayer, Enabling paddleRightPlayer");
                paddleLeftPlayer.enabled = false;
                paddleRightPlayer.enabled = true;
            }
        }
    }

    void Update()
    {
    }
}