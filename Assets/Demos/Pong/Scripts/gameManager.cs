// using UnityEngine;
// using Mirror
// using System;

// public class GameManager : MonoBehaviour
// {
//     public GameObject ballPrefab;
//     public Transform ballSpawnPoint;

//     private GameObject ballInstance;
//     private int player1Score = 0;
//     private int player2Score = 0;

//     public enum GameState { Waiting, Playing, Ended }
//     public GameState currentState = GameState.Waiting;

//     void Start()
//     {
//         StartGame();
//     }

//     void StartGame()
//     {
//         currentState = GameState.Playing;
//         SpawnBall();
//         UIManager.Instance.UpdateGameState(currentState);
//     }

//     void SpawnBall()
//     {
//         ballInstance = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
//     }

//     public void ResetBall()
//     {
//         Destroy(ballInstance);
//         SpawnBall();
//     }

//     public void UpdateScore(int playerID)
//     {
//         if (playerID == 1)
//         {
//             player1Score++;
//         }
//         else if (playerID == 2)
//         {
//             player2Score++;
//         }
//         Debug.Log($"Player 1 Score: {player1Score} | Player 2 Score: {player2Score}");
//         UIManager.Instance.UpdateScores(player1Score, player2Score);
//         ResetBall();
//     }

//     public void EndGame()
//     {
//         currentState = GameState.Ended;
//         UIManager.Instance.UpdateGameState(currentState);
//         Debug.Log("Game ended!");
//     }
// }
