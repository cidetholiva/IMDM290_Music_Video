using UnityEngine;
public class CirclePath : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject circlePrefab;
    public float distance;
    public float speed;
    public int numCircles;
    GameObject[] circles;
    float[] t;
    public void Start()
    {
        circles = new GameObject[numCircles];
        t = new float[numCircles];
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i] = Instantiate(circlePrefab, transform);
            circles[i].GetComponent<Circle>().audioSource = audioSource;
            t[i] = ((float)i / (float)numCircles);
        }
    }
    public void Update()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            t[i] = ((float)i / (float)numCircles) + (Time.time * speed);
            t[i] = t[i] % 1;

            circles[i].transform.localPosition = Vector3.forward * t[i] * distance;
        }
    }
}
