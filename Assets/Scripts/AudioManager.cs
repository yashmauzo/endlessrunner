using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;  // Array to hold all sound objects used in the game
    private AudioSource laneAudioSource;  // Reference to the AudioSource for the lane background music
    private Sound warningSound;           // Reference to the AudioSource for the lane background music

    // Start is called before the first frame update
    void Start()
    {
        // Loop through all sounds and initialize each sound's AudioSource component
        foreach (Sound s in sounds)
        {
            // Create a new AudioSource for each sound and assign its clip and properties
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;     // Set looping if applicable

            // Set spatial blend to control 3D sound behavior (0 for 2D, 1 for 3D)
            s.source.spatialBlend = s.spatialBlend;  // SpatialBlend should be a property in the Sound class

            // Assign the background music to the laneAudioSource and start playing it
            if (s.name == "LaneMusic")
            {
                laneAudioSource = s.source;  // Store the lane music source reference
                laneAudioSource.Play();      // Start playing the lane music
            }

            // Store reference to the warning sound for later use
            if (s.name == "WarningSound")
            {
                warningSound = s;  // Store the reference to the warning sound object
            }
        }
    }

    // Play a sound by name
    public void PlaySound(string name)
    {
        // Find the sound by its name and play it
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Play();
        }
    }

    // Play the warning sound for obstacles, adjusting its volume based on proximity
    public void PlayWarningSound(float volume)
    {
        // Ensure the warning sound is available and not already playing
        if (warningSound != null && !warningSound.source.isPlaying)
        {
            warningSound.source.volume = volume;  // Adjust the volume based on proximity to the obstacle
            warningSound.source.Play();     // Play the warning sound
        }
    }

    // Stop the warning sound if it's playing
    public void StopWarningSound()
    {
        // Ensure the warning sound is available and currently playing
        if (warningSound != null && warningSound.source.isPlaying)
        {
            warningSound.source.Stop();     // Stop the warning sound
        }
    }

    // Stop any sound by name
    public void StopSound(string name)
    {
        // Find the sound by its name and stop it
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Stop();
        }
    }

    // Update the stereo panning for the lane music, allowing sound to come from left or right
    public void SetLaneAudioPan(float panValue)
    {
        // Ensure the laneAudioSource is available and update its panning
        if (laneAudioSource != null)
        {
            laneAudioSource.panStereo = panValue;   // Set pan value (-1 for left ear, 1 for right ear)
        }
    }
}
