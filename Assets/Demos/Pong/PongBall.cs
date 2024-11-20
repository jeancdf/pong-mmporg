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

    void Start()
    {
        Debug.Log("PongBall Start");
        InitializeDirection();
    }

    void Update()
    {
        transform.position += direction * Speed * Time.deltaTime;
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
            direction = Vector3.Reflect(direction, normal).normalized;
        }
    }

    private void InitializeDirection()
    {
        // Randomly choose a horizontal direction
        float horizontalDirection = Random.value < 0.5f ? 1f : -1f;
        // Add a random vertical component
        float verticalDirection = Random.Range(-0.5f, 0.5f);

        direction = new Vector3(horizontalDirection, verticalDirection, 0).normalized;
    }
}
