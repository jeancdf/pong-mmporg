using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField ipInput;
    public Button hostButton;
    public Button connectButton;

    void Start()
    {
        hostButton.onClick.AddListener(OnHostButton);
        connectButton.onClick.AddListener(OnConnectButton);
    }

    void OnHostButton()
    {
        networkManager.StartHost();
        hostButton.interactable = false;
        connectButton.interactable = false;
    }

    void OnConnectButton()
    {
        if (string.IsNullOrEmpty(ipInput.text))
        {
            Debug.LogError("Please enter an IP address!");
            return;
        }

        networkManager.ConnectToHost(ipInput.text);
        hostButton.interactable = false;
        connectButton.interactable = false;
    }
} 