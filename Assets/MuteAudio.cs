using UnityEngine;
using TMPro;

public class MuteUnmuteAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isMuted = false;

    public TMP_Text buttonText;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;

        // Change le texte du bouton
        buttonText.text = isMuted ? "Unmute" : "Mute";
    }
}
