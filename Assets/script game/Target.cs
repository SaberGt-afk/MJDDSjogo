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
    public static bool hitByLastArrow = false;
    private bool isGameOver = false;
    private float gameOverDelay = 0.1f;

    private AudioSource hitSound; // Referência para o AudioSource

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
        hitSound = GetComponent<AudioSource>(); // Obtém o AudioSource do alvo
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Pontuação: " + score);

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }

        if (score >= scoreToNextScene && !isGameOver)
        {
            isGameOver = true;
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

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "level 1")
        {
            SceneManager.LoadScene("level 2");
        }
        else if (currentSceneName == "level 2")
        {
            SceneManager.LoadScene("menu");
        }
        else
        {
            Debug.LogWarning("Cena desconhecida. Carregando cena padrão.");
            SceneManager.LoadScene("menu");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            AddScore(10);
            hitByLastArrow = true;
            if (hitSound != null)
            {
                hitSound.Play(); // Toca o som quando a flecha acerta
            }
            Destroy(other.gameObject);
        }
    }

    public void CheckEndCondition()
    {
        if (isGameOver) return;
        hitByLastArrow = false;

        if (score < scoreToNextScene)
        {
            StartCoroutine(DelayedFailCheck());
        }
    }

    private IEnumerator DelayedFailCheck()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (!isGameOver)
        {
            isGameOver = true;
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

    public int GetScore()
    {
        return score;
    }
}