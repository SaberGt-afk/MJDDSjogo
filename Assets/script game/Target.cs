using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Target : MonoBehaviour
{
    private int score = 0;
    public int scoreToNextScene = 50;

    public GameObject pngImage;     // imagem de sucesso
    public GameObject failImage;    // imagem de falha
    public float delayBeforeSceneChange = 2f;

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Pontuação: " + score);

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
            Debug.Log("Tentativas esgotadas. A reiniciar cena em 5 segundos...");

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
}
