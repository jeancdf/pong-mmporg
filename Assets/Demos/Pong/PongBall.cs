using UnityEngine;

public UDPSender Sender;
public enum PongBallState {
  Playing = 0,
  PlayerLeftWin = 1,
  PlayerRightWin = 2,
}

public class PongBall : MonoBehaviour
{
    public float Speed = 1;

    Vector3 Direction;
    PongBallState _State = PongBallState.Playing;

    public PongBallState State {
      get {
        return _State;
      }
    } 

    void Start() {
      Direction = new Vector3(
        Random.Range(0.5f, 1),
        Random.Range(-0.5f, 0.5f),
        0
      );
      Direction.x *= Mathf.Sign(Random.Range(-100, 100));
      Direction.Normalize();
    }

    void Update() {
      if (State != PongBallState.Playing) {
        return;
      }

      transform.position = transform.position + (Direction * Speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision c) {
      switch (c.collider.name) {
        case "BoundTop":
        case "BoundBottom":
          Direction.y = -Direction.y;
          break;

        case "PaddleLeft":
        case "PaddleRight":
          Direction.x = -Direction.x;
          message = "Collision paddle"
          Sender.DestinationIP = IP;
          Sender.DestinationPort = port;
          Sender.SendUDPMessage(message);
          break;

        case "BoundLeft":
          _State = PongBallState.PlayerRightWin;
          message = "Player Right win"
          Sender.DestinationIP = IP;
          Sender.DestinationPort = port;
          Sender.SendUDPMessage(message);
          break;

        case "BoundRight":
          _State = PongBallState.PlayerLeftWin;
          message = "Player Left win"
          Sender.DestinationIP = IP;
          Sender.DestinationPort = port;
          Sender.SendUDPMessage(message);
          break;

      }
    }

}
