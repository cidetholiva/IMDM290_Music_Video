using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    GameObject[] segments;
    public float speed = 10;
    public float radius = 7;
    public int segmentsNum = 15;
    public float segmentSize = 1;
    public AudioSource audioSource;
    void Start()
    {
        segments = new GameObject[segmentsNum];
        for (int i = 0; i < segmentsNum; i++)
        {
            segments[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            segments[i].transform.SetParent(transform);
            segments[i].GetComponent<MeshRenderer>().material.color = Color.HSVToRGB((float)i / segmentsNum, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float amp = AudioSpectrumUtil.GetAmp(audioSource);
        for (int i = 0; i < segmentsNum; i++)
        {
            float angle = (Time.time * 2 * Mathf.PI * speed) + (2 * Mathf.PI * i / segmentsNum);
            segments[i].transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            segments[i].transform.localScale = Vector3.one * segmentSize * amp;
        }
    }
}
