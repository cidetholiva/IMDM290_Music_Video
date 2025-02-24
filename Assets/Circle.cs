
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Circle : MonoBehaviour
{
    public AudioSource audioSource;
    public int detail = 100;
    public float lineWidth = 1f;
    public float radius = 5f;
    public Color color = Color.red;
    LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = lineRenderer.endColor = color;
        lineRenderer.positionCount = detail;
        for (int i = 0; i < detail; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(i * 2 * Mathf.PI / detail), Mathf.Sin(i * 2 * Mathf.PI / detail), 0) * radius);
        }
    }
    void Update()
    {
        float amp = audioSource == null? 1 : AudioSpectrumUtil.GetAmp(audioSource);
        for (int i = 0; i < detail; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(i * 2 * Mathf.PI / detail), Mathf.Sin(i * 2 * Mathf.PI / detail), 0) * (radius * amp));
        }
    }
}
