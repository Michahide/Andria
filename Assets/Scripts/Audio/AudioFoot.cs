using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFoot : MonoBehaviour
{
    public AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    public void FootAudio(){
        aud.Play();
    }
}
