using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // public AudioSource warningAudioSource;  // Global warning audio source for obstacles
    private AudioSource laneAudioSource;  // Reference to the single AudioSource for lane music
    private Sound warningSound;           // Reference to the warning sound in the sounds array

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            // Set spatialBlend based on whether it's a 2D or 3D sound
            s.source.spatialBlend = s.spatialBlend;  // Make sure Sound class has a spatialBlend property

            // Check for the LaneMusic sound
            if (s.name == "LaneMusic")
            {
                laneAudioSource = s.source;  // Assign the lane music to this source
                laneAudioSource.Play();      // Start playing the music
            }

            // Check for the WarningSound and store its reference
            if (s.name == "WarningSound")
            {
                warningSound = s;  // Store the reference to the warning sound
            }
        }
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Play();
        }
    }

    public void PlayWarningSound(float volume)
    {
        if (warningSound != null && !warningSound.source.isPlaying)
        {
            warningSound.source.volume = volume;  // Set volume based on proximity
            warningSound.source.Play();
        }
    }

    public void StopWarningSound()
    {
        if (warningSound != null && warningSound.source.isPlaying)
        {
            warningSound.source.Stop();
        }
    }

    public void StopSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Stop();
        }
    }

    // Method to update the stereo pan of the lane music
    public void SetLaneAudioPan(float panValue)
    {
        if (laneAudioSource != null)
        {
            laneAudioSource.panStereo = panValue;
        }
    }
}
