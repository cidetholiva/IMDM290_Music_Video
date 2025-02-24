using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StopStartTime());
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Time.deltaTime * 100, Time.deltaTime * 100, Time.deltaTime * 100);
    }

    IEnumerator StopStartTime()
    {
        for (int i = 0; i < 10; i++)
        {
            Time.timeScale = 1; // Set time scale to normal.
            yield return new WaitForSecondsRealtime(1); // Wait for 1 second.
            Time.timeScale = 0; // Set time scale to freeze.
            yield return new WaitForSecondsRealtime(1); // Wait for 1 second.
        }
    }
}
