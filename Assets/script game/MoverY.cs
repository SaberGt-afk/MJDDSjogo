using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverY : MonoBehaviour
{
     public float velocidade = 2f; // Velocidade de movimento no eixo Y
    public float limiteSuperior = 5f; // Limite superior do movimento
    public float limiteInferior = -5f; // Limite inferior do movimento
    private bool aSubir = true; // Indica se o objeto está a subir

    void Update()
    {
        // Verifica se o objeto atingiu o limite superior ou inferior
        if (transform.position.y > limiteSuperior)
        {
            aSubir = false; // Começa a descer
        }
        else if (transform.position.y < limiteInferior)
        {
            aSubir = true; // Começa a subir
        }

        // Move o objeto no eixo Y dependendo da direção
        if (aSubir)
        {
            transform.Translate(Vector3.up * velocidade * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * velocidade * Time.deltaTime);
        }
    }
}
