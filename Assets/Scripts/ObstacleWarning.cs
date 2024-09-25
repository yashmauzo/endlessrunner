using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWarning : MonoBehaviour
{
    // public AudioSource warningAudioSource; // The warning audio source for the obstacle
    public float warningDistance = 200f;    // Distance at which the warning should start
    public float vibrationDistance = 30f;   // Distance at which haptic feedback should trigger
    public float safeDistanceAfterPass = 20f; // Extra distance after crossing to stop the sound
    private Transform player;              // Reference to the player's position
    private PlayerController playerController; // Reference to the player's lane info
    private AudioManager audioManager;     // Reference to the AudioManager
    private bool isWarningPlaying = false;
    private static ObstacleWarning closestObstacle = null;  // Static reference to the closest obstacle
    private bool isVibrating = false;  // Track if the haptic feedback is already triggered


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
        // Check if the game has started
        if (!PlayerManager.isGameStarted)
        {
            StopWarningSound();  // Ensure warning sounds are not playing before the game starts
            StopVibration();     // Stop vibration when the game hasn't started
            return;
        }

        if (player != null && playerController != null)
        {
            // Calculate the Z distance to the player
            float distanceToPlayerZ = transform.position.z - player.position.z;

            // Check if the player is in the same lane and approaching the obstacle
            if (IsPlayerInSameLane() && IsPlayerApproaching() && IsClosestObstacle())
            {
                PlayWarningSound();
                TriggerHapticFeedback(distanceToPlayerZ);  // Trigger haptic feedback if close
            }
            else if (!IsPlayerInSameLane() || isWarningPlaying && !IsPlayerPastObstacle())
            {
                // If the player is no longer in the same lane, stop the sound
                StopWarningSound();
                StopVibration();  // Stop vibration if not in the same lane or if past the obstacle
            }
            else
            {
                StopWarningSound();
                StopVibration();
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

    bool IsPlayerPastObstacle()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;

        // Return true if the player has crossed the obstacle (distance becomes negative or beyond the safe distance)
        return distanceToPlayerZ < -safeDistanceAfterPass;
    }

    // Check if this obstacle is the closest one in the same lane as the player
    bool IsClosestObstacle()
    {
        float distanceToPlayerZ = transform.position.z - player.position.z;

        // If this obstacle is in the same lane and there is no closest obstacle, or if this obstacle is closer, update the reference
        if (IsPlayerInSameLane() && (closestObstacle == null || distanceToPlayerZ < closestObstacle.GetDistanceToPlayer()))
        {
            closestObstacle = this;  // Set this as the closest obstacle in the player's lane
            return true;
        }

        return closestObstacle == this;  // This is the closest obstacle if it's already set as such
    }

    float GetDistanceToPlayer()
    {
        return Mathf.Abs(transform.position.z - player.position.z);
    }

    // Notify the AudioManager to play the warning sound
    void PlayWarningSound()
    {
        if (!isWarningPlaying)
        {
            float distanceToPlayerZ = transform.position.z - player.position.z;
            float volume = Mathf.Lerp(1.0f, 0.0f, distanceToPlayerZ / warningDistance);
            audioManager.PlayWarningSound(volume);
            isWarningPlaying = true;
        }
    }

    // Notify the AudioManager to stop the warning sound
    void StopWarningSound()
    {
        if (isWarningPlaying)
        {
            audioManager.StopWarningSound();
            isWarningPlaying = false;
        }
    }

    // Trigger haptic feedback (vibration) based on proximity to the obstacle
    void TriggerHapticFeedback(float distanceToPlayerZ)
    {
        if (!isVibrating && distanceToPlayerZ <= vibrationDistance)
        {
            Handheld.Vibrate();  // Trigger the vibration on mobile devices
            isVibrating = true;  // Mark vibration as triggered
        }
    }

    // Stop vibration when no longer necessary
    void StopVibration()
    {
        if (isVibrating)
        {
            isVibrating = false;  // Reset vibration trigger (vibration will stop automatically after a short time)
        }
    }
}