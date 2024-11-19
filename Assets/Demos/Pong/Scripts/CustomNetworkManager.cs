using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public int player1Score = 0;
    public int player2Score = 0;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        Debug.Log("Player added. Total players: " + numPlayers);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Player disconnected. Total players: " + numPlayers);
    }

    public void UpdateScore(int playerNumber, int score)
    {
        if (playerNumber == 1)
        {
            player1Score += score;
        }
        else if (playerNumber == 2)
        {
            player2Score += score;
        }
        Debug.Log("Player 1 Score: " + player1Score + " | Player 2 Score: " + player2Score);
    }

    public void StartGame()
    {
        // Logic to start the game
        Debug.Log("Game started!");
    }

    public void EndGame()
    {
        // Logic to end the game
        Debug.Log("Game ended!");
    }
} 