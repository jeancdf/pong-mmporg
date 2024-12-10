using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    public float speed = 5f;
    public float minY = -4f;
    public float maxY = 4f;

    public bool isLocalPlayer; // Ce paddle est contrôlé par le joueur local
    private bool roleAssigned = false; // Vérifie si le rôle a été attribué

    private PongInput inputActions; // Instance des contrôles générés
    private InputAction moveAction; // Action spécifique pour ce paddle

    void Start()
    {
        inputActions = new PongInput();
    }

    void Update()
    {
        if (!roleAssigned) return; // Ignore les contrôles si le rôle n'a pas été attribué

        if (!isLocalPlayer) return; // Ignore les contrôles si ce n'est pas le joueur local

        float direction = moveAction.ReadValue<float>(); // Lire la direction d'entrée
        Move(direction);    
    }

    void Move(float direction)
    {
        Vector3 newPos = transform.position + (Vector3.up * speed * direction * Time.deltaTime);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        transform.position = newPos;
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.Disable();
    }

    // Méthode appelée lorsque le rôle est attribué
    public void OnRoleAssigned(string role)
    {
        if (role == "Player1" && gameObject.name == "PaddleLeft")
        {
            isLocalPlayer = true;
            moveAction = inputActions.Pong.Player1;
        }
        else if (role == "Player2" && gameObject.name == "PaddleRight")
        {
            isLocalPlayer = true;
            moveAction = inputActions.Pong.Player2;
        }
        else
        {
            isLocalPlayer = false; // Ce paddle n'est pas contrôlé par le joueur local
        }

        moveAction?.Enable(); // Activer l'action si ce paddle est contrôlé par le joueur local
        roleAssigned = true; // Le rôle a été attribué

        Debug.Log($"Rôle attribué à {gameObject.name}: {role}");
    }
}
