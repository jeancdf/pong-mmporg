using UnityEngine;
using UnityEngine.SceneManagement;

public class PongWinUI : MonoBehaviour
{
    public GameObject Panel;
    public GameObject PlayerLeft;
    public GameObject PlayerRight;
    public NetworkManager networkManager;

    private PongBall Ball;
    private float searchInterval = 0.5f;
    private float nextSearchTime = 0f;

    void Start()
    {
        if (networkManager == null)
            networkManager = FindObjectOfType<NetworkManager>();

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
            Ball = FindObjectOfType<PongBall>();
            nextSearchTime = Time.time + searchInterval;
            return;
        }

        // Skip update if we don't have the ball or any UI elements
        if (Ball == null || Panel == null || PlayerLeft == null || PlayerRight == null)
            return;

        UpdateUI();
    }

    void UpdateUI()
    {
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
                break;
            case PongBallState.PlayerRightWin:
                Panel.SetActive(true);
                PlayerLeft.SetActive(false);
                PlayerRight.SetActive(true);
                break;
        }
    }

    public void OnReplay()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
