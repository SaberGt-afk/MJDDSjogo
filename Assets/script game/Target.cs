using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Importa SceneManager

public class Target : MonoBehaviour
{
    private int score = 0;
    public int scoreToNextScene = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            score += 10;
            Debug.Log("Pontuação: " + score);

            Destroy(other.gameObject);

            if (score >= scoreToNextScene)
            {
                SceneManager.LoadScene("level 2");
            }
        }
    }
}
