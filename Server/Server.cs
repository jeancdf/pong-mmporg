using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace pong_mmporg.Server
{
    string IpPlayer1;
    string IpPlayer2;

    public class Server : MonoBehaviour
    {
        void Start()
        {
            Task.Run(() => StartServer());
        }

        void StartServer()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 1234);
            server.Start();

            TcpClient client1 = server.AcceptTcpClient();
            IpPlayer1 = ((IPEndPoint)client1.Client.RemoteEndPoint).Address.ToString();
            TcpClient client2 = server.AcceptTcpClient();
            IpPlayer2 = ((IPEndPoint)client2.Client.RemoteEndPoint).Address.ToString();
        }

        void Update()
        {
            if (IpPlayer1 != null && IpPlayer2 != null)
            {
                Task.Run(() => StartGame());
            }
        }

        void StartGame()
        {
            Game game = new Game(IpPlayer1, IpPlayer2);
            game.Start();
            Task.Run(() => StartUdpCommunication());
        }

        void StartUdpCommunication()
        {
            UdpClient udpClient = new UdpClient(1235);
            IPEndPoint remoteEndPoint1 = new IPEndPoint(IPAddress.Parse(IpPlayer1), 1235);
            IPEndPoint remoteEndPoint2 = new IPEndPoint(IPAddress.Parse(IpPlayer2), 1235);

            while (true)
            {
                byte[] receivedData = udpClient.Receive(ref remoteEndPoint1);

                byte[] sendData = new byte[] {};
                udpClient.Send(sendData, sendData.Length, remoteEndPoint1);
                udpClient.Send(sendData, sendData.Length, remoteEndPoint2);
            }
        }
    }
}