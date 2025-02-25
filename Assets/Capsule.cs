using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    public AudioSource audioSource;       
    public float beatAmp = 0.05f;     
    public float spikeSize = 3f;     //depends on beat
    public int spikeCount = 8;              

    private GameObject capsule;              
    private GameObject[] spikes;         

    void Start()
    {
       //pill shape in the middle- https://docs.unity3d.com/ScriptReference/PrimitiveType.Capsule.html
        capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.transform.position = Vector3.zero;
        capsule.transform.localScale = new Vector3(2f, 2f, 2f); // increase size 

        
        // creating "spikes" around pill-https://docs.unity3d.com/ScriptReference/Transform-parent.html
        spikes = new GameObject[spikeCount];
        for (int i = 0; i < spikeCount; i++)
        {
            spikes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); //spikes made of cubes
            spikes[i].transform.parent = capsule.transform;
            spikes[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
            float angle = 360f / spikeCount * i; //spikes aorund
            float radians = angle * (Mathf.PI / 180f); // degree--to radians
            Vector3 direction = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));

            
            float capsuleRadius = 0.5f; //position inside pill
            float offset = capsuleRadius + 0.1f;
            spikes[i].transform.localPosition = direction * offset;
            spikes[i].transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction);
            //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Quaternion.html
        }
    }

    void Update()
    {
        //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/AudioSource.GetSpectrumData.html
        float[] spectrumData = new float[64];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float amplitude = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            amplitude += spectrumData[i];
        }
        float rotationSpeed = Mathf.Clamp(amplitude * 200f, 10f, 200f); // rotation speed 
        capsule.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);


        

        // color cycle
        Color capsuleColor = Color.HSVToRGB((Time.time * 0.5f) % 1, 1, 1);
        capsule.GetComponent<Renderer>().material.color = capsuleColor;

        // spike size based on amp.
        for (int i = 0; i < spikeCount; i++)
        {
            Renderer spikeRenderer = spikes[i].GetComponent<Renderer>();
            Vector3 direction = spikes[i].transform.localPosition.normalized;
            
            if (amplitude > beatAmp) //// makes spike longer based on beat
            {
                float spikeLength = 0.2f + amplitude * spikeSize;
                spikes[i].transform.localScale = new Vector3(0.2f, spikeLength, 0.2f);
                float capsuleRadius = 0.5f;
                float offset = capsuleRadius + spikeLength / 2f;
                spikes[i].transform.localPosition = direction * offset;
                spikeRenderer.material.color = capsuleColor; //spike color match pill
            }
            else
            {
                // goes to default size/position
                Vector3 defaultScale = new Vector3(0.2f, 0.2f, 0.2f);
                spikes[i].transform.localScale = Vector3.Lerp(spikes[i].transform.localScale, defaultScale, Time.deltaTime * 5f);
                float capsuleRadius = 0.5f;
                float defaultOffset = capsuleRadius + 0.1f;
                spikes[i].transform.localPosition = Vector3.Lerp(spikes[i].transform.localPosition, direction * defaultOffset, Time.deltaTime * 5f);
                spikeRenderer.material.color = Color.Lerp(spikeRenderer.material.color, capsuleColor, Time.deltaTime * 5f);
            }
        }
    }
}
