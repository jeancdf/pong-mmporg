using UnityEngine;
using Mirror;

public enum PongBallState
{
    Playing,
    PlayerLeftWin,
    PlayerRightWin
}

public class PongBall : NetworkBehaviour
{
    public float Speed = 5f;
    
    [SyncVar]
    public PongBallState State = PongBallState.Playing;
    
    private Vector3 direction;
    private Rigidbody rb;

    void Start()
    {
        if (!isServer) return;
        
        rb = GetComponent<Rigidbody>();
        direction = Random.value < 0.5f ? Vector3.right : Vector3.left;
        rb.linearVelocity = direction * Speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;

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
}
