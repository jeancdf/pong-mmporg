using UnityEngine;

public enum PongBallState
{
    Playing,
    PlayerLeftWin,
    PlayerRightWin
}

public class PongBall : MonoBehaviour
{
    public float Speed = 5f;
    
    public PongBallState State = PongBallState.Playing;
    
    private Vector3 direction;

    [SerializeField]
    private NetworkManager networkManager; // Référence au NetworkManager


    void Start()
    {
        Debug.Log("PongBall Start");
        InitializeDirection();
    }

    void Update()
    {
        transform.position += direction * Speed * Time.deltaTime;
        SendBallPositionToServer();
    }

    void Update()
    {
        // Transmet la position de la balle au serveur
        SendBallPositionToServer();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "BoundLeft")
        {
            State = PongBallState.PlayerRightWin;
            Debug.Log("PongBall: PlayerRightWin");
        }
        else if (collision.gameObject.name == "BoundRight")
        {
            State = PongBallState.PlayerLeftWin;
            Debug.Log("PongBall: PlayerLeftWin");
        }
        else
        {
            Vector3 normal = collision.contacts[0].normal;
            direction = Vector3.Reflect(direction, normal);
            rb.linearVelocity = direction * Speed;
        }
    }


    private void SendBallPositionToServer()
    {
        if (networkManager != null)
        {
            Vector3 position = transform.position;
            networkManager.SendBallPosition(position);
        }
    }
}
