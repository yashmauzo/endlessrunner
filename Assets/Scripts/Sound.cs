using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sound
{
    public string name;
    public AudioClip clip;

    public float volume;

    public bool loop;

    [Range(0f, 1f)]
    public float spatialBlend;  // Add spatialBlend property here

    public AudioSource source;
}
