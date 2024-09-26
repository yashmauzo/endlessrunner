using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWarning : MonoBehaviour
{
    public float warningDistance = 200f;    // Distance at which the warning sound should start playing
    public float vibrationDistance = 30f;   // Distance at which haptic feedback (vibration) should trigger
    public float safeDistanceAfterPass = 20f; // Distance after passing the obstacle when the warning should stop
    private Transform player;              // Reference to the player's position
    private PlayerController playerController; // Reference to the PlayerController script for lane info
    private AudioManager audioManager;     // Reference to the AudioManager for sound control
    private bool isWarningPlaying = false;      // Track whether the warning sound is currently playing
    private static ObstacleWarning closestObstacle = null;  // Static reference to keep track of the closest obstacle
    private bool isVibrating = false;  // Track whether the haptic feedback (vibration) is already triggered


    // Called when the script starts, initializes player and AudioManager references
    void Start()
    {
        // Find the player in the scene using its tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        audioManager = FindObjectOfType<AudioManager>(); // Get the AudioManager reference from the scene

        // Check if player is found
        if (playerObject != null)
        {
            player = playerObject.transform;        // Get the player's transform for position tracking
            playerController = playerObject.GetComponent<PlayerController>();      // Get lane information from the PlayerController script

            // If the PlayerController script is not found, display an error
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

    // Called once per frame, checks the player's position and updates the warning sound or haptic feedback
    void Update()
    {
        // Stop all warnings if the game has not started yet
        if (!PlayerManager.isGameStarted)
        {
            StopWarningSound();  // Ensure warning sound is stopped
            StopVibration();     // Ensure vibration is stopped
            return;     // Exit the function if the game hasn't started
        }

        // Proceed if the player is found
        if (player != null && playerController != null)
        {
            // Calculate the Z-axis distance between the obstacle and the player
            float distanceToPlayerZ = transform.position.z - player.position.z;

            // Check if the player is in the same lane, approaching, and if this is the closest obstacle
            if (IsPlayerInSameLane() && IsPlayerApproaching() && IsClosestObstacle())
            {
                PlayWarningSound();     // Play obstacle warning sound if conditions are met
                TriggerHapticFeedback(distanceToPlayerZ);  // Trigger haptic feedback based on proximity
            }
            // If the player moves out of the lane or is no longer approaching the obstacle, stop warnings
            else if (!IsPlayerInSameLane() || isWarningPlaying && !IsPlayerPastObstacle())
            {
                StopWarningSound();     // Stop sound if no longer relevant
                StopVibration();  // Stop vibration if no longer necessary
            }
            else
            {
                StopWarningSound();     // Ensure warning sound stops
                StopVibration();        // Ensure vibration stops
            }
        }
    }

    // Check if the player is in the same lane as the obstacle
    bool IsPlayerInSameLane()
    {
        float laneDistance = playerController.laneDistance;     // Distance between lanes
        float distanceToPlayerX = Mathf.Abs(transform.position.x - player.position.x);      // Horizontal distance

        // Return true if the player is in the same lane (small difference in X position)
        return distanceToPlayerX < laneDistance / 2;
    }

    // Check if the player is approaching the obstacle within the warning distance
    bool IsPlayerApproaching()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;

        // Return true if the obstacle is ahead of the player and within the warning distance
        return distanceToPlayerZ > 0 && distanceToPlayerZ < warningDistance;
    }

    // Check if the player has passed the obstacle completely
    bool IsPlayerPastObstacle()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;

        // Return true if the player has crossed the obstacle (Z distance becomes negative or beyond the safe distance)
        return distanceToPlayerZ < -safeDistanceAfterPass;
    }

    // Check if this is the closest obstacle in the player's current lane
    bool IsClosestObstacle()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;

        // Return true if this is the closest obstacle in the lane
        if (IsPlayerInSameLane() && (closestObstacle == null || distanceToPlayerZ < closestObstacle.GetDistanceToPlayer()))
        {
            closestObstacle = this;  // Mark this obstacle as the closest
            return true;
        }

        return closestObstacle == this;  // Return true if this is already marked as the closest obstacle
    }

    // Get the Z distance between this obstacle and the player
    float GetDistanceToPlayer()
    {
        return Mathf.Abs(transform.position.z - player.position.z);
    }

    // Play warning sound based on proximity to the player
    void PlayWarningSound()
    {
        // If warning sound is not already playing, play it and adjust volume based on distance
        if (!isWarningPlaying)
        {
            float distanceToPlayerZ = transform.position.z - player.position.z;
            float volume = Mathf.Lerp(1.0f, 0.0f, distanceToPlayerZ / warningDistance);     // Calculate volume based on distance
            audioManager.PlayWarningSound(volume);      // Play warning sound through AudioManager
            isWarningPlaying = true;        // Mark that warning sound is playing
        }
    }

    // Stop the warning sound
    void StopWarningSound()
    {
        // Stop warning sound and reset flag
        if (isWarningPlaying)
        {
            audioManager.StopWarningSound();    // Stop the sound via AudioManager
            isWarningPlaying = false;       // Mark that the warning sound is stopped
        }
    }

    // Trigger haptic feedback (vibration) when the player is close enough to the obstacle
    void TriggerHapticFeedback(float distanceToPlayerZ)
    {
        // Trigger vibration only if it's not already triggered and the player is within the vibration distance
        if (!isVibrating && distanceToPlayerZ <= vibrationDistance)
        {
            Handheld.Vibrate();  // Trigger vibration on mobile devices
            isVibrating = true;  // Mark vibration as triggered
        }
    }

    // Stop haptic feedback
    void StopVibration()
    {
        // Reset vibration state (vibration stops automatically on most devices)
        if (isVibrating)
        {
            isVibrating = false;  // Reset vibration trigger
        }
    }
}