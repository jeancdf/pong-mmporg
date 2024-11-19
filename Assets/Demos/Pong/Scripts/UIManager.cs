// using UnityEngine;
// using UnityEngine.UI;

// public class UIManager : MonoBehaviour
// {
//     public static UIManager Instance;

//     public Text player1ScoreText;
//     public Text player2ScoreText;
//     public Text gameStateText;

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     public void UpdateScores(int player1Score, int player2Score)
//     {
//         player1ScoreText.text = "Player 1: " + player1Score;
//         player2ScoreText.text = "Player 2: " + player2Score;
//     }

//     public void UpdateGameState(GameManager.GameState state)
//     {
//         gameStateText.text = "Game State: " + state.ToString();
//     }
// } 