using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public enum PongPlayer {
  PlayerLeft = 1,
  PlayerRight = 2
}

public class PongPaddle : NetworkBehaviour
{ 
    public PongPlayer Player = PongPlayer.PlayerLeft;
    public float Speed = 1;
    public float MinY = -4;
    public float MaxY = 4;

    PongInput inputActions;
    InputAction PlayerAction;

    void Start()
    {
        if (!isLocalPlayer) return;

        inputActions = new PongInput();
        switch (Player) {
            case PongPlayer.PlayerLeft:
                PlayerAction = inputActions.Pong.Player1;
                break;
            case PongPlayer.PlayerRight:
                PlayerAction = inputActions.Pong.Player2;
                break;
        }

        PlayerAction.Enable();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        float direction = PlayerAction.ReadValue<float>();
        CmdMove(direction);
    }

    [Command]
    void CmdMove(float direction)
    {
        Vector3 newPos = transform.position + (Vector3.up * Speed * direction * Time.deltaTime);
        newPos.y = Mathf.Clamp(newPos.y, MinY, MaxY);
        transform.position = newPos;
    }

    void OnDisable() 
    {
        if (PlayerAction != null)
            PlayerAction.Disable();
    }
}
