using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radio : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] songs;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        index = Random.Range(0, songs.Length);
        source.clip = songs[index];
        source.Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) {
            index++;
            if (index >= songs.Length) index = 0;
            source.clip = songs[index];
            source.Play();
        }
    }
}
