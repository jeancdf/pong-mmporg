using UnityEngine;
using UnityEngine.InputSystem;

public enum PongPlayer {
    PlayerLeft = 1,
    PlayerRight = 2
}

public class PongPaddle : MonoBehaviour
{ 
    public PongPlayer Player = PongPlayer.PlayerLeft;
    public float Speed = 1;
    public float MinY = -4;
    public float MaxY = 4;

    public bool isLocalPlayer = false; // Cette variable détermine si ce paddle est contrôlé par le joueur local

    PongInput inputActions;
    InputAction PlayerAction;

    void Start()
    {
        inputActions = new PongInput();

        // Détermine l'action en fonction du rôle du joueur local (Player1 ou Player2)
        if (Globals.Player == "Player1" && gameObject.name == "PaddleLeft")
        {
            isLocalPlayer = true; // Le joueur local contrôle LeftPaddle
            PlayerAction = inputActions.Pong.Player1;
        }
        else if (Globals.Player == "Player2" && gameObject.name == "PaddleRight")
        {
            isLocalPlayer = true; // Le joueur local contrôle RightPaddle
            PlayerAction = inputActions.Pong.Player2;
        }
        else
        {
            isLocalPlayer = false; // Ce paddle n'est pas contrôlé par le joueur local
        }

        PlayerAction?.Enable(); // N'active les actions que pour le joueur local
    }

    void Update()
    {
        // Si ce n'est pas le joueur local, ne pas exécuter de contrôle
        if (!isLocalPlayer) return;

        // Lire la direction d'entrée et déplacer le paddle
        float direction = PlayerAction.ReadValue<float>();
        Move(direction);    
    }

    void Move(float direction)
    {
        // Déplace le paddle verticalement dans les limites définies
        Vector3 newPos = transform.position + (Vector3.up * Speed * direction * Time.deltaTime);
        newPos.y = Mathf.Clamp(newPos.y, MinY, MaxY);
        transform.position = newPos;
    }

    void OnDisable()
    {
        // Désactive les actions quand le paddle est désactivé
        if (PlayerAction != null)
            PlayerAction.Disable();
    }

    
}
