using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;     // Reference to the CharacterController component
    private Vector3 direction;      // The movement direction vector
    public float forwardSpeed;      // Speed at which the player moves forward

    private int desiredLane = 1; //0:left 1:middle 2:right // Keeps track of which lane the player should be in
    public float laneDistance = 4; //the distance between two lanes

    public float jumpForce;     // The force applied when the player jumps
    public float Gravity = -20;   // The gravity affecting the player (negative for downward force)

    private AudioManager audioManager;  // Reference to the AudioManager script to control sounds

    void Start()
    {
        // Get the CharacterController component attached to the player object
        controller = GetComponent<CharacterController>();

        // Get the AudioManager from the scene to manage audio feedback
        audioManager = FindObjectOfType<AudioManager>();  // Get the AudioManager reference
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game has started, if not, don't run this code
        if (!PlayerManager.isGameStarted)
            return;

        // Always move the player forward
        direction.z = forwardSpeed;

        // Check if the player is on the ground and swipe up to jump
        if (controller.isGrounded)
        {
            if (SwipeManager.swipeUp)       // Detect upward swipe
            {
                Jump();     // Trigger jump action
            }
        }
        else
        {
            // Apply gravity if the player is not on the ground
            direction.y += Gravity * Time.deltaTime;
        }


        // Handle player lane switching

        //if(Input.GetKeyDown(KeyCode.RightArrow))

        // Swipe right to move to the right lane
        if (SwipeManager.swipeRight)
        {
            desiredLane++;      // Move to the right lane
            if (desiredLane == 3)
                desiredLane = 2;    // Ensure player doesn't go beyond the right-most lane
            UpdateLaneAudioPan();  // Update audio based on lane
        }

        //if(Input.GetKeyDown(KeyCode.LeftArrow))

        // Swipe left to move to the left lane
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;      // Move to the left lane
            if (desiredLane == -1)
                desiredLane = 0;    // Ensure player doesn't go beyond the left-most lane
            UpdateLaneAudioPan();  // Update audio based on lane
        }

        // Calculate the target position based on the lane the player is in
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        // Adjust the position based on the lane (move left or right)
        if (desiredLane == 0)   // Left lane
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)  // Right lane
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // Move towards the target position smoothly
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;    // Calculate the movement direction
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);       // Move if not at the target yet
        else
            controller.Move(diff);      // Stop when at the target
    }

    // Update audio panning based on the lane the player is in
    private void UpdateLaneAudioPan()
    {
        float panValue = 0f;  // Default to center lane (both ears)

        // Adjust panning based on the lane the player is in
        switch (desiredLane)
        {
            case 0:  // Left lane
                panValue = -1f;  // Play in the left ear only
                break;
            case 1:  // Center lane
                panValue = 0f;   // Play in both ears
                break;
            case 2:  // Right lane
                panValue = 1f;   // Play in the right ear only
                break;
        }

        // Call the AudioManager to update the stereo pan
        audioManager.SetLaneAudioPan(panValue);
    }

    // FixedUpdate is called at regular intervals to handle physics-related updates
    private void FixedUpdate()
    {
        // Check if the game has started, if not, don't run this code
        if (!PlayerManager.isGameStarted)
            return;

        // Gradually increase the forward speed of the player
        forwardSpeed += 0.1f * Time.fixedDeltaTime;
        // Move the player forward in the current direction
        controller.Move(direction * Time.fixedDeltaTime);
    }

    // Handle jump action
    private void Jump()
    {
        direction.y = jumpForce;    // Apply the jump force in the Y direction
    }

    // Detect when the player collides with an obstacle
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If the player hits an obstacle, trigger the game over state
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;  // Mark the game as over
            FindObjectOfType<AudioManager>().PlaySound("GameOver");     // Play game over sound
        }
    }
}
