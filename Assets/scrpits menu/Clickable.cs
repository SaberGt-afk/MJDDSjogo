using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickable : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f; // valor mínimo de opacidade para ser clicável

    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
    }
}
