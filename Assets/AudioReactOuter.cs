// UMD IMDM290
// Instructor: Myungin Lee
// Original code provided by Myungin Lee 
// Modified by Cideth Oliva 
    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactOuter : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 2000; // increased for smoother shape
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;
    float minScale = 0.5f; //for pulse (smallest scale)
    float maxScale = 3f; // for pulse (laregest scale)
    float beatThreshold = 0.8f;   // amp value--beat detection
    float beatBoost = 1.5f; //boost scale on beat


    // Start is called before the first frame update
    void Start()
    {
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i = 0; i < numSphere; i++){
            // Random start positions
            float r = 10f; // reduced for closer initial positions
            startPosition[i] = Random.insideUnitSphere * r;
            
            // star shape end position
            t = i * 2 * Mathf.PI / numSphere;
            float starRadius = 5f * (1 + 0.5f * Mathf.Cos(5 * t)); // 0.5 spike 
            float x = starRadius * Mathf.Cos(t);
            float y = starRadius * Mathf.Sin(t);
            float z = 0f; // flat
            endPosition[i] = new Vector3(x, y, z) * 0.2f; // star size (small)
        }

        // Let there be spheres..
        for (int i = 0; i < numSphere; i++){
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            spheres[i].transform.localScale = Vector3.one * Random.Range(0.03f, 0.1f); //size of circles (small/big)

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];

            // Color
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;

            //  trail renderer for smoother effect and extra animation
            //https://docs.unity3d.com/ScriptReference/TrailRenderer.html
            TrailRenderer trail = spheres[i].AddComponent<TrailRenderer>(); //as each sphere moves, leaves a trail
            trail.time = 0.2f;
            trail.startWidth = 0.05f;
            trail.endWidth = 0.01f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
        }

        // adjusts camera position, so shape can be easily seen
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        //https://docs.unity3d.com/ScriptReference/Mathf.PingPong.html
        //creates line that goes back and forth, creates the "breathing" effect
        float baseScale = Mathf.Lerp(minScale, maxScale, AudioSpectrum.audioAmp);
        float currentScale = (AudioSpectrum.audioAmp > beatThreshold) ? baseScale * beatBoost : baseScale;

        


        // Measure Time 
        time += Time.deltaTime;

        for (int i = 0; i < numSphere; i++)
        {
            // makes the back and forth motion smoother
            //https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
            lerpFraction = Mathf.SmoothStep(0, 1, Mathf.PingPong(time * 0.5f, 1));

            // update position
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction) * currentScale;

            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();

            //colors defined
            Color hotPink = new Color(1f, 0f, 0.5f);
            Color lightPink = new Color(1f, 0.75f, 0.9f);
            Color purple = new Color(0.5f, 0f, 0.5f);

            float cycle = Mathf.PingPong(time, 1f);
            Color baseColor = (cycle < 0.5f) ? Color.Lerp(hotPink, purple, cycle * 2f) : Color.Lerp(purple, lightPink, (cycle - 0.5f) * 2f);

            Color finalColor = (AudioSpectrum.audioAmp > beatThreshold) ? Color.Lerp(baseColor, Color.white, 0.5f) : baseColor;

            sphereRenderer.material.color = finalColor;

            
            // updates trail color
            TrailRenderer trail = spheres[i].GetComponent<TrailRenderer>();
            trail.material.color = sphereRenderer.material.color;
        }
    }
}