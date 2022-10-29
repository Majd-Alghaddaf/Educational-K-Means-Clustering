using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public Color color;

    private void Start()
    {
        UpdateColor();
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void UpdateColor()
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }
}
