using UnityEngine;

public enum PongBallState
{
    Playing,
    PlayerLeftWin,
    PlayerRightWin
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

    void Awake() {
      if (!Globals.IsServer) {
        enabled = false;
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

    void OnCollisionEnter(Collision c) {
      switch (c.collider.name) {
        case "BoundTop":
        case "BoundBottom":
          Direction.y = -Direction.y;
          break;

        case "PaddleLeft":
        case "PaddleRight":
        case "BoundLeft":
        case "BoundRight":
          Direction.x = -Direction.x;
          break;

        /*
        case "BoundLeft":
          _State = PongBallState.PlayerRightWin;
          break;

        case "BoundRight":
          _State = PongBallState.PlayerLeftWin;
          break;
        */

      }
    }
}
