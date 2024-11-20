using UnityEngine;
using UnityEngine.InputSystem;

public enum PongPlayer {
  PlayerLeft = 1,
  PlayerRight = 2
}

public class PongPaddle : MonoBehaviour
{ 
    [SerializeField]
    private NetworkManager networkManager;
    public PongPlayer Player = PongPlayer.PlayerLeft;
    public float Speed = 1;
    public float MinY = -4;
    public float MaxY = 4;

    PongInput inputActions;
    InputAction PlayerAction;

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

    // Connexion ou démarrage d'une partie via NetworkManager
    if (networkManager != null)
    {
        networkManager.ConnectToHost("100.72.128.85"); // Remplacez par l'IP du serveur si nécessaire
        //networkManager.SendMessageToServer("Player Right connected.");
    
    }
    else
    {
        Debug.LogError("NetworkManager n'est pas assigné dans PongPaddle !");
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

        // Envoi de la position actuelle du paddle
        //if (networkManager != null)
        //{
        //    networkManager.SendPaddlePosition(transform.position.y);
        //}

        }

    void OnDisable()
    {
    if (PlayerAction != null)
        PlayerAction.Disable();
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
