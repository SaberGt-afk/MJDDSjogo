using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text tentativasText;

    private int score;
    private int tentativas;

    public void UpdateScore(int newScore)
    {
        score = newScore;
        if (scoreText != null)
            scoreText.text = "Pontos: " + score;
    }

    public void UpdateTentativas(int novasTentativas)
    {
        tentativas = novasTentativas;
        if (tentativasText != null)
            tentativasText.text = "Tentativas: " + tentativas;
    }
}
