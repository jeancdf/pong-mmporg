using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class NetworkManager : MonoBehaviour
{
    private TcpListener server;
    private TcpClient client;
    private NetworkStream stream;
    private Thread networkThread;
    private bool isHost = false;
    private bool isConnected = false;
    private const int PORT = 7777;

    public PongPaddle leftPaddle;
    public PongPaddle rightPaddle;
    public PongBall ball;

    void Start()
    {
        // Initialize components
        if (leftPaddle == null || rightPaddle == null || ball == null)
        {
            Debug.LogError("Please assign paddle and ball references in inspector!");
            return;
        }
    }

    public void StartHost()
    {
        try
        {
            server = new TcpListener(IPAddress.Any, PORT);
            server.Start();
            isHost = true;
            Debug.Log("Server started on port " + PORT);

            networkThread = new Thread(new ThreadStart(ServerLoop));
            networkThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Error starting server: " + e.Message);
        }
    }

    public void ConnectToHost(string ipAddress)
    {
        try
        {
            client = new TcpClient();
            client.Connect(ipAddress, PORT);
            stream = client.GetStream();
            isConnected = true;
            Debug.Log("Connected to server!");

            networkThread = new Thread(new ThreadStart(ClientLoop));
            networkThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    private void ServerLoop()
    {
        try
        {
            client = server.AcceptTcpClient();
            stream = client.GetStream();
            isConnected = true;
            Debug.Log("Client connected!");

            byte[] buffer = new byte[1024];
            while (isConnected)
            {
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessNetworkData(data);
                }
                Thread.Sleep(16); // ~60fps
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Server error: " + e.Message);
        }
    }

    private void ClientLoop()
    {
        try
        {
            byte[] buffer = new byte[1024];
            while (isConnected)
            {
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    ProcessNetworkData(data);
                }
                Thread.Sleep(16); // ~60fps
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Client error: " + e.Message);
        }
    }

    private void ProcessNetworkData(string data)
    {
        // Format: "paddle:position" or "ball:positionX,positionY"
        string[] parts = data.Split(':');
        if (parts.Length != 2) return;

        switch (parts[0])
        {
            case "paddle":
                float paddleY = float.Parse(parts[1]);
                if (!isHost)
                    UpdatePaddlePosition(rightPaddle, paddleY);
                else
                    UpdatePaddlePosition(leftPaddle, paddleY);
                break;

            case "ball":
                if (!isHost) // Only host controls ball
                {
                    string[] pos = parts[1].Split(',');
                    float ballX = float.Parse(pos[0]);
                    float ballY = float.Parse(pos[1]);
                    UpdateBallPosition(ballX, ballY);
                }
                break;
        }
    }

    public void SendPaddlePosition(float position)
    {
        if (!isConnected) return;
        string data = $"paddle:{position}";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        stream.Write(buffer, 0, buffer.Length);
    }

    public void SendBallPosition(Vector3 position)
    {
        if (!isConnected || stream == null) return;

        string data = $"ball:{position.x},{position.y}";
        byte[] buffer = Encoding.ASCII.GetBytes(data);

        try
        {
            stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors de l'envoi de la position de la balle : " + e.Message);
        }
    }

    private void UpdatePaddlePosition(PongPaddle paddle, float yPosition)
    {
        // Use main thread for Unity operations
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Vector3 pos = paddle.transform.position;
            pos.y = yPosition;
            paddle.transform.position = pos;
        });
    }

    private void UpdateBallPosition(float x, float y)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            ball.transform.position = new Vector3(x, y, 0);
        });
    }

    void OnDestroy()
    {
        isConnected = false;
        if (networkThread != null)
            networkThread.Abort();
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
        if (server != null)
            server.Stop();
    }

    void Update()
    {
        if (isHost && isConnected)
        {
            SendBallPosition(ball.transform.position);
        }
    }
} 