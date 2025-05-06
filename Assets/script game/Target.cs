using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Target : MonoBehaviour
{
    private int score = 0;
    public int scoreToNextScene = 50;

    public GameObject pngImage;
    public GameObject failImage;
    public float delayBeforeSceneChange = 2f;

    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Pontuação: " + score);

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }

        if (score >= scoreToNextScene)
        {
            StartCoroutine(ShowPNGThenLoadScene());
        }
    }

    private IEnumerator ShowPNGThenLoadScene()
    {
        if (pngImage != null)
        {
            pngImage.SetActive(true);
        }
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene("level 2");
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
            Debug.Log("Pontuação insuficiente. A reiniciar cena em 5 segundos...");

            if (failImage != null)
            {
                failImage.SetActive(true);
            }

            StartCoroutine(RestartSceneAfterDelay(5f));
        }
    }

    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetScore() // Adicionado: Método para obter a pontuação
    {
        return score;
    }
}