// UMD IMDM290 
// Instructor: Myungin Lee
// All the same Lerp but using audio
// Modified by: Cideth Oliva 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactInner : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 150; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        

            r = 3f; // radius of the circle
            // Circular end position
            float t = i * 2 * Mathf.PI / numSphere;
            float starRadius = r * (1 + 0.4f * Mathf.Pow(Mathf.Cos(5 * t), 1)); // smoother star shape
            endPosition[i] = new Vector3(starRadius * Mathf.Cos(t), starRadius * Mathf.Sin(t), 0) * 0.5f;



        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];

            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ***Here, we use audio Amplitude, where else do you want to use?
        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?
        time += Time.deltaTime * (AudioSpectrum.audioAmp * 0.1f);

        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numSphere;
            spheres[i].transform.position += (endPosition[i] - spheres[i].transform.position) * (AudioSpectrum.audioAmp * 0.5f); //beat effect

            float scale = (Mathf.Sin(time * 2f + i * 0.1f) * 0.5f + 1.5f + AudioSpectrum.audioAmp * 2f) * 0.3f; //updated so spheres don't get too big on beat change
            spheres[i].transform.localScale = new Vector3(scale, scale, scale);
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);
            
            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            sphereRenderer.material.color = Color.Lerp(new Color(0f, 1f, 1f), new Color(1f, 0f, 0.5f), Mathf.Clamp01(AudioSpectrum.audioAmp * 0.3f));
        }
    }
}
