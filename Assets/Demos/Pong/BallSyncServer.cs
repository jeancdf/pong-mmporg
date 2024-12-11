using UnityEngine;

public class BallSyncServer : MonoBehaviour
{
    ServerManager ServerMan;
    float NextUpdateTimeout = -1;

    void Awake() {
      if (!Globals.IsServer) {
        enabled = false;
      }
    }

    void Start()
    {
        ServerMan = FindFirstObjectByType<ServerManager>();
    }

    void Update()
    {
        if (Time.time > NextUpdateTimeout)
        {
            Vector3 position = transform.position;

            string data = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "X:{0},Y:{1},Z:{2}",
                position.x, position.y, position.z);
            string message = $"UPDATE|BALL|{data}";

            ServerMan.BroadcastUDPMessage(message);

            NextUpdateTimeout = Time.time + 0.01f;
        }
    }
}