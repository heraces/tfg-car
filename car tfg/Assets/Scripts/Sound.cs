using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource idle;
    private float idleMaxVolume;
    private float idleMaxPitch;

    [SerializeField] private AudioSource running;
    private float runningMaxVolume = .8f;
    private float runningMinVolume = .05f;

    private Engine car;
    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<Engine>();
    }

    // Update is called once per frame
    void Update()
    {

        running.pitch = car.rpm / 8000 * 3 +.1f;
        if (Input.GetKey(KeyCode.W)) running.volume = Mathf.Lerp(running.volume, runningMaxVolume, Time.deltaTime);
        else running.volume = Mathf.Lerp(running.volume, runningMinVolume, Time.deltaTime);
    }
}
