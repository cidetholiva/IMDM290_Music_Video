using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class Rotate : MonoBehaviour
{
    public AudioSource audioSource;
    public float size = 3;
    public float colorSpeed = 0.25f;
    void Update()
    {
        gameObject.transform.Rotate(Time.deltaTime * 100, Time.deltaTime * 100, Time.deltaTime * 100);
        gameObject.transform.localScale = Vector3.one * size * AudioSpectrumUtil.GetAmp(audioSource);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB((Time.time * colorSpeed) % 1, 1, 1);
    }
}

public static class AudioSpectrumUtil
{
    public static int FFTSIZE = 1024; // https://en.wikipedia.org/wiki/Fast_Fourier_transform
    public static float[] samples = new float[FFTSIZE];
    public static float GetAmp(AudioSource audioSource)
    {
        // The source (time domain) transforms into samples in frequency domain 
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Hanning);
        // Empty first, and pull down the value.
        float audioAmp = 0f;
        for (int i = 0; i < FFTSIZE; i++)
        {
            audioAmp += samples[i];
        }
        return audioAmp;
    }
}