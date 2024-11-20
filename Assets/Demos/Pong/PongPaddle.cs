using UnityEngine;
using UnityEngine.InputSystem;

public enum PongPlayer {
  PlayerLeft = 1,
  PlayerRight = 2
}

public class PongPaddle : MonoBehaviour
{ 
    [SerializeField]
    public TCPClient tcpClient;
    public PongPlayer Player = PongPlayer.PlayerLeft;
    public float Speed = 1;
    public float MinY = -4;
    public float MaxY = 4;

    PongInput inputActions;
    InputAction PlayerAction;
    NetworkManager networkManager;

    void Start()
    {
        // Trouve le NetworkManager (si nécessaire pour d'autres fonctionnalités)
    networkManager = FindObjectOfType<NetworkManager>();
    inputActions = new PongInput();

    switch (Player)
    {
        case PongPlayer.PlayerLeft:
            PlayerAction = inputActions.Pong.Player1;
            break;
        case PongPlayer.PlayerRight:
            PlayerAction = inputActions.Pong.Player2;
            break;
    }

    PlayerAction.Enable();

    // Initialisation de la connexion TCP
    if (tcpClient != null)
    {
        // Connexion au serveur TCP
        tcpClient.Connect(OnServerMessageReceived);

        // Vérifiez si la connexion a réussi
        if (tcpClient.IsConnected)
        {
            Debug.Log("Connexion au serveur réussie !");
            // Envoyer un message au serveur
            tcpClient.SendTCPMessage(Player + " connected.");
        }
        else
        {
            Debug.LogWarning("Échec de la connexion au serveur.");
        }
    }
    else
    {
        Debug.LogError("TCPClient n'est pas assigné dans l'inspecteur !");
    }
    }

    void Update()
    {
        float direction = PlayerAction.ReadValue<float>();
        Move(direction);
    }

    void Move(float direction)
    {
        Vector3 newPos = transform.position + (Vector3.up * Speed * direction * Time.deltaTime);
        newPos.y = Mathf.Clamp(newPos.y, MinY, MaxY);
        transform.position = newPos;

        if (networkManager != null)
        {
            networkManager.SendPaddlePosition(transform.position.y);
        }
    }

    void OnDisable() 
    {
        if (PlayerAction != null)
            PlayerAction.Disable();
        if (tcpClient != null && tcpClient.IsConnected)
    {
        tcpClient.SendTCPMessage("Player " + Player + " disconnected.");
        tcpClient.Close();
    }
    }

    private void OnServerMessageReceived(string message)
{
    Debug.Log("Message reçu du serveur : " + message);

    // Exemple : démarrez une action en fonction du message reçu
    if (message == "StartGame")
    {
        Debug.Log("La partie commence !");
    }
}
}
