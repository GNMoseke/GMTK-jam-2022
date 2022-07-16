using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialColor : MonoBehaviour
{
    private Color randColor;
    void Start()
    {
        randColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        this.gameObject.GetComponent<Renderer>().material.color = randColor;
    }
}
