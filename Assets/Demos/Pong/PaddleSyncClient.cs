using System;
using System.Net;
using UnityEngine;
using System.Globalization;

public class PaddleSyncClient : MonoBehaviour
{
    public enum PaddleSide { LEFT, RIGHT }
    public PaddleSide paddleSide; // À définir dans l'Inspector pour chaque paddle
    private float lastPositionY;
    private const float positionThreshold = 0.01f; 

    private UDPService udpService;
    private ClientManager clientManager;

    void Awake()
    {
        if (Globals.IsServer)
        {
            enabled = false;
            return;
        }

        udpService = FindObjectOfType<UDPService>();
        udpService.OnMessageReceived += OnMessageReceived;
        clientManager = FindObjectOfType<ClientManager>();
    }

    private void OnMessageReceived(string message, IPEndPoint sender)
    {
        if (!message.StartsWith("UPDATE|PADDLE|")) return;

        try
        {
            string[] tokens = message.Split('|');
            if (tokens.Length < 4)
            {
                Debug.LogWarning("Message mal formé : parties insuffisantes.");
                return;
            }

            string receivedPaddleSide = tokens[2];
            if (receivedPaddleSide != paddleSide.ToString()) return; // Ignorer si ce n'est pas le paddle correspondant

            string data = tokens[3];
            string[] positionData = data.Split(':');

            if (positionData.Length != 2 || positionData[0] != "Y")
            {
                Debug.LogWarning($"Données de position mal formées : {data}");
                return;
            }

            float y = float.Parse(positionData[1], CultureInfo.InvariantCulture);

            Vector3 newPosition = transform.position;
            newPosition.y = y;
            transform.position = newPosition;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"Erreur lors du parsing des données de position : {ex.Message}");
        }
    }

    void Update()
    {
        float currentPositionY = transform.position.y;
        if (Mathf.Abs(currentPositionY - lastPositionY) > positionThreshold)
        {
            string message = $"UPDATE|PADDLE|{paddleSide}|Y:{currentPositionY.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
            udpService.SendUDPMessage(message, clientManager.ServerEndpoint);
            Debug.Log(
                $"Message envoyé par le client {clientManager.ServerEndpoint.Address}:{clientManager.ServerEndpoint.Port} => {message}");
            lastPositionY = currentPositionY;
        }
    }
}