using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text P1ScoreText; // Référence au texte pour le score du joueur 1
    public TMP_Text P2ScoreText; // Référence au texte pour le score du joueur 2

    void Update()
    {
        // Met à jour les champs de texte avec les scores actuels
        Debug.Log("Globals.P1Score");
        P1ScoreText.text = Globals.P1Score.ToString();
        P2ScoreText.text = Globals.P2Score.ToString();
        Debug.Log("P1ScoreText.text :" + P1ScoreText.text);
        Debug.Log("P2ScoreText.text :" + P2ScoreText.text);
    }
}
