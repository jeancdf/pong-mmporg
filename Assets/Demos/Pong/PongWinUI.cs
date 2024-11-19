using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PongWinUI : MonoBehaviour
{
    public GameObject Panel;
    public GameObject PlayerLeft;
    public GameObject PlayerRight;

    private PongBall Ball;
    private float searchInterval = 0.5f;
    private float nextSearchTime = 0f;

    void Awake()
    {
        Debug.Log("PongWinUI: Awake called");
    }

    void Start()
    {
        Debug.Log("PongWinUI: Start called");
        
        if (Panel == null) Debug.LogError("PongWinUI: Panel is not assigned!");
        if (PlayerLeft == null) Debug.LogError("PongWinUI: PlayerLeft is not assigned!");
        if (PlayerRight == null) Debug.LogError("PongWinUI: PlayerRight is not assigned!");

        // Make sure UI elements start hidden
        if (Panel != null) Panel.SetActive(false);
        if (PlayerLeft != null) PlayerLeft.SetActive(false);
        if (PlayerRight != null) PlayerRight.SetActive(false);
    }

    void Update()
    {
        // Try to find ball periodically if we don't have it
        if (Ball == null && Time.time >= nextSearchTime)
        {
            Debug.Log("PongWinUI: Searching for ball...");
            Ball = FindObjectOfType<PongBall>();
            if (Ball != null)
                Debug.Log("PongWinUI: Ball found!");
            nextSearchTime = Time.time + searchInterval;
            return;
        }

        // Skip update if we don't have the ball or any UI elements
        if (Ball == null || Panel == null || PlayerLeft == null || PlayerRight == null)
        {
            return;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        Debug.Log($"PongWinUI: Updating UI. Ball State: {Ball.State}");
        
        switch (Ball.State)
        {
            case PongBallState.Playing:
                Panel.SetActive(false);
                PlayerLeft.SetActive(false);
                PlayerRight.SetActive(false);
                break;
            case PongBallState.PlayerLeftWin:
                Panel.SetActive(true);
                PlayerLeft.SetActive(true);
                PlayerRight.SetActive(false);
                Debug.Log("PongWinUI: Player Left Wins!");
                break;
            case PongBallState.PlayerRightWin:
                Panel.SetActive(true);
                PlayerLeft.SetActive(false);
                PlayerRight.SetActive(true);
                Debug.Log("PongWinUI: Player Right Wins!");
                break;
        }
    }

    public void OnReplay()
    {
        Debug.Log("PongWinUI: Replay button clicked");
        if (NetworkServer.active)
        {
            NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
        }
    }
}
