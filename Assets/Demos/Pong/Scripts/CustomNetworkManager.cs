using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public bool debugMode = true;

    public override void Awake()
    {
        base.Awake();
        Debug.Log("CustomNetworkManager: Awake");
    }

    public override void Start()
    {
        base.Start();
        Debug.Log("CustomNetworkManager: Start");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("CustomNetworkManager: Server Started");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("CustomNetworkManager: Client Started");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        Debug.Log($"CustomNetworkManager: Player Added. Total players: {numPlayers}");
        
        // Assign player positions based on connection order
        GameObject player = conn.identity.gameObject;
        PongPaddle paddle = player.GetComponent<PongPaddle>();
        
        if (numPlayers == 1)
        {
            paddle.Player = PongPlayer.PlayerLeft;
            player.transform.position = new Vector3(-5, 0, 0);

            if (debugMode)
            {
                Debug.Log("Debug mode: Spawning right paddle and ball");
                // Spawn right paddle for debugging
                GameObject rightPaddle = Instantiate(playerPrefab, new Vector3(5, 0, 0), Quaternion.identity);
                rightPaddle.GetComponent<PongPaddle>().Player = PongPlayer.PlayerRight;
                NetworkServer.Spawn(rightPaddle);

                // Spawn ball
                SpawnBall();
            }
        }
        else if (numPlayers == 2)
        {
            paddle.Player = PongPlayer.PlayerRight;
            player.transform.position = new Vector3(5, 0, 0);
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        if (NetworkServer.active)
        {
            Debug.Log("Spawning ball...");
            GameObject ball = Instantiate(spawnPrefabs.Find(prefab => prefab.GetComponent<PongBall>() != null));
            NetworkServer.Spawn(ball);
        }
    }
} 