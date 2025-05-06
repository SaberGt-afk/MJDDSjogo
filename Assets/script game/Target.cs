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
    public static bool hitByLastArrow = false; // Novo booleano estático
    private bool isGameOver = false; // Nova variável para controlar o fim de jogo
    private float gameOverDelay = 0.1f; // Pequeno atraso para a derrota

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

        if (score >= scoreToNextScene && !isGameOver) // Verifica se o jogo não acabou
        {
            isGameOver = true; // Marca o fim de jogo
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
            hitByLastArrow = true; // Define como verdadeiro se atingido
            Destroy(other.gameObject);
        }
    }

    public void CheckEndCondition()
    {
        if (isGameOver) return; // Se o jogo já acabou, não faz nada
        hitByLastArrow = false; // Reset no início

        if (score < scoreToNextScene)
        {
            StartCoroutine(DelayedFailCheck()); // Usa a coroutine com atraso
        }
    }

    private IEnumerator DelayedFailCheck()
    {
        yield return new WaitForSeconds(gameOverDelay); // Espera um pouco

        if (!isGameOver) // Verifica novamente se o jogo não acabou entretanto
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