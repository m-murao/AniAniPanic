using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeAutoDestroy : MonoBase
{
    private AudioSource audio = null;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        if (!audio)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (!audio.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
