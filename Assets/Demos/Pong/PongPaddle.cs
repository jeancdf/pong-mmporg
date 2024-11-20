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
        // Find the NetworkManager (if needed for other functionalities)
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

        // Bypass TCP connection logic for now
        if (tcpClient != null)
        {
            Debug.Log("TCPClient is assigned, but skipping connection setup.");
        }
        else
        {
            Debug.LogWarning("TCPClient is not assigned in the inspector!");
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
        Debug.Log("Message received from server: " + message);

        // Example: start an action based on the received message
        if (message == "StartGame")
        {
            Debug.Log("The game is starting!");
        }
    }
}
