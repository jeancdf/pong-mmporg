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
    private Rigidbody rb;

    [SerializeField]
    private NetworkManager networkManager; // Référence au NetworkManager


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = Random.value < 0.5f ? Vector3.right : Vector3.left;
        rb.linearVelocity = direction * Speed;
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
        }
        else if (collision.gameObject.name == "BoundRight")
        {
            State = PongBallState.PlayerLeftWin;
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
