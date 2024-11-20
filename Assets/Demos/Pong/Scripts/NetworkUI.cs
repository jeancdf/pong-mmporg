using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;
using System.Net.Sockets;

public class NetworkUI : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField ipInput;
    public Button hostButton;
    public Button connectButton;
    public Button localPlayButton;
    public TextMeshProUGUI statusText;

    void Start()
    {
        Debug.Log("NetworkUI Start");
        if (networkManager == null)
        {
            Debug.LogError("NetworkManager is not assigned!");
            return;
        }

        if (ipInput == null)
        {
            Debug.LogError("IP Input Field is not assigned!");
            return;
        }

        if (hostButton == null)
        {
            Debug.LogError("Host Button is not assigned!");
            return;
        }

        if (connectButton == null)
        {
            Debug.LogError("Connect Button is not assigned!");
            return;
        }

        if (localPlayButton == null)
        {
            Debug.LogError("Local Play Button is not assigned!");
            return;
        }

        hostButton.onClick.AddListener(OnHostButton);
        connectButton.onClick.AddListener(OnConnectButton);
        localPlayButton.onClick.AddListener(OnLocalPlayButton);
    }

    void OnHostButton()
    {
        Debug.Log("NetworkUI: Host button clicked");

        if (networkManager != null)
        {
            networkManager.StartHost(); // Start the server
            string localIP = GetLocalIPAddress();
            statusText.text = $"Hosting on {localIP}:{networkManager.Port}";
            HideNetworkUI(); // Hide the UI after starting the host
        }
        else
        {
            Debug.LogError("NetworkManager is not assigned!");
            statusText.text = "Error: NetworkManager not assigned!";
        }

        hostButton.interactable = false;
        connectButton.interactable = false;
    }

    void OnConnectButton()
    {
        if (string.IsNullOrEmpty(ipInput.text))
        {
            Debug.LogError("Please enter an IP address!");
            statusText.text = "Error: Please enter an IP address!";
            return;
        }

        networkManager.ConnectToHost(ipInput.text);
        statusText.text = $"Connecting to {ipInput.text}:{networkManager.Port}";
        HideNetworkUI(); // Hide the UI after connecting
    }

    void OnLocalPlayButton()
    {
        Debug.Log("NetworkUI: Local play button clicked");
        statusText.text = "Playing locally.";
        HideNetworkUI(); // Hide the UI for local play
        // Additional setup for local play can be added here if needed
    }

    private void HideNetworkUI()
    {
        Debug.Log("NetworkUI: HideNetworkUI");
        GameObject canvas = GameObject.Find("Canvas"); // Find the Canvas GameObject
        if (canvas != null)
        {
            canvas.SetActive(false); // Deactivate the Canvas GameObject
        }
        else
        {
            Debug.LogError("Canvas GameObject not found!");
        }
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public void OnStopButton()
    {
        if (networkManager != null)
        {
            networkManager.StopNetwork();
            statusText.text = "Disconnected.";
            gameObject.SetActive(true); // Reactivate the UI if needed
        }

        hostButton.interactable = true;
        connectButton.interactable = true;
    }
} 