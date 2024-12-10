using System.Net;
using UnityEngine;
using System.Globalization;

public class BallSyncClient : MonoBehaviour
{
    UDPService UDP;

    void Awake() {
      if (Globals.IsServer) {
        enabled = false;
      }
    }

    void Start()
    {
        UDP = FindFirstObjectByType<UDPService>();

        UDP.OnMessageReceived += (string message, IPEndPoint sender) =>
        {
            if (!message.StartsWith("UPDATE|BALL|")) { return; }

            try
            {
                string[] tokens = message.Split('|');
                if (tokens.Length < 3)
                {
                    Debug.LogWarning("Message mal formé : parties insuffisantes.");
                    return;
                }

                string data = tokens[2];
                

                string[] positionData = data.Split(',');
                if (positionData.Length != 3)
                {
                    Debug.LogWarning($"Données de position mal formées : coordonnées insuffisantes : {data}");
                    Debug.LogWarning($"Raw position data: {data}"); 
                    return;
                }

                
                float x = float.Parse(positionData[0].Split(':')[1], CultureInfo.InvariantCulture);
                float y = float.Parse(positionData[1].Split(':')[1], CultureInfo.InvariantCulture);
                float z = float.Parse(positionData[2].Split(':')[1], CultureInfo.InvariantCulture);

                transform.position = new Vector3(x, y, z);
                
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Erreur lors du parsing des données de position : {ex.Message}");
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
