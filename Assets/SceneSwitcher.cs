using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject scene1;
    public GameObject scene2;
    public GameObject scene3;
    public GameObject scene4;
    public enum Awaiting
    {
        SecondSwitch,
        ThirdSwitch,
        FourthSwitch,
        Done
    }
    Awaiting awaiting = Awaiting.SecondSwitch;
    void Start()
    {
        scene1.SetActive(true);
        audioSource.Play();
    }
    private void Update()
    {
        float firstSwitchTime = 53.5f;
        float secondSwitchTime = 80f;
        float thirdSwitchTime = 60f + 46.5f;
        if (audioSource.time >= firstSwitchTime && awaiting == Awaiting.SecondSwitch)
        {
            scene1.SetActive(false);
            scene2.SetActive(true);
            awaiting = Awaiting.ThirdSwitch;
        }
        else if (audioSource.time >= secondSwitchTime && awaiting == Awaiting.ThirdSwitch)
        {
            scene2.SetActive(false);
            scene3.SetActive(true);
            awaiting = Awaiting.FourthSwitch;
        }
        else if (audioSource.time >= thirdSwitchTime && awaiting == Awaiting.FourthSwitch)
        {
            scene3.SetActive(false);
            scene4.SetActive(true);
            awaiting = Awaiting.Done;
        }
    }
}
