using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Target targetScript;
    [SerializeField] private Reticle reticleScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (reticleScript != null && reticleScript.shotsLeft <= 0 &&
            targetScript != null && targetScript.GetScore() < targetScript.scoreToNextScene)
        {
            Debug.Log("DeathZone ativada. A reiniciar cena...");
            targetScript.CheckEndCondition();
        }
    }
}