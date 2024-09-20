using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWarning : MonoBehaviour
{
    // public AudioSource warningAudioSource; // The warning audio source for the obstacle
    public float warningDistance = 15f;    // Distance at which the warning should start
    private Transform player;              // Reference to the player's position
    private PlayerController playerController; // Reference to the player's lane info
    private AudioManager audioManager;     // Reference to the AudioManager


    void Start()
    {
        // Find the player and the AudioManager in the scene
        GameObject playerObject = GameObject.FindWithTag("Player");
        audioManager = FindObjectOfType<AudioManager>(); // Find the AudioManager

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();

            // Check if PlayerController is attached
            if (playerController == null)
            {
                Debug.LogError("PlayerController script not found on the Player object!");
            }
        }
        else
        {
            Debug.LogError("Player object not found! Make sure the Player is tagged correctly.");
        }
    }

    void Update()
    {
        if (player != null && playerController != null)
        {
            if (IsPlayerInSameLane() && IsPlayerApproaching())
            {
                PlayWarningSound();
            }
            else
            {
                StopWarningSound();
            }
        }
    }

    // Check if the player is in the same lane as the obstacle
    bool IsPlayerInSameLane()
    {
        float laneDistance = playerController.laneDistance;
        float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);

        // Return true if the player is in the same lane (small difference on the X axis)
        return distanceToPlayerX < laneDistance / 2;
    }

    // Check if the player is approaching the obstacle (within the warning distance)
    bool IsPlayerApproaching()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;

        // If the obstacle is ahead of the player and within the warning distance
        return distanceToPlayerZ > 0 && distanceToPlayerZ < warningDistance;
    }

    // Notify the AudioManager to play the warning sound
    void PlayWarningSound()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;
        float volume = Mathf.Lerp(1.0f, 0.0f, distanceToPlayerZ / warningDistance);  // Volume based on proximity
        audioManager.PlayWarningSound(volume);  // Notify AudioManager to play the sound
    }

    // Notify the AudioManager to stop the warning sound
    void StopWarningSound()
    {
        audioManager.StopWarningSound();  // Notify AudioManager to stop the sound
    }
}