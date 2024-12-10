using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public TMPro.TMP_InputField InpIP;
    
    public void setIpAddress() {
        Globals.IPAddress = InpIP.text;
    }

    public void SetRole(bool isServer) {
        Globals.IsServer = isServer;
    }

    public void StartGame() {
        if (Globals.IsServer) {
            SceneManager.LoadScene("Pong");
        }
        if(Globals.IPAddress != "" && Globals.IsServer == false) {
            SceneManager.LoadScene("Pong");
        }
        

    }
}
