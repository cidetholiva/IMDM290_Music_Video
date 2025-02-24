using UnityEngine;
public class CirclePath : MonoBehaviour
{
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
            t[i] = (i / distance);
        }
    }
    public void Update()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            t[i] += Time.deltaTime * speed;
            if (t[i] >= 1f) t[i] = 0f;

            circles[i].transform.localPosition = Vector3.forward * t[i] * distance;
        }
    }
}
