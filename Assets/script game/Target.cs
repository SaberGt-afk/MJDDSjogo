using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    private int score = 0;
    public int scoreToNextScene = 50;

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Pontuação: " + score);

        if (score >= scoreToNextScene)
        {
            SceneManager.LoadScene("level 2");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            AddScore(10);
            Destroy(other.gameObject);
        }
    }

    public void CheckEndCondition()
    {
        if (score < scoreToNextScene)
        {
            Debug.Log("Tentativas esgotadas. Reiniciando cena...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
