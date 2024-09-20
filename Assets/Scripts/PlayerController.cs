using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1; //0:left 1:middle 2:right
    public float laneDistance = 4; //the distance between two lanes

    public float jumpForce;
    public float Gravity = -20;

    private AudioManager audioManager;  // Reference to the AudioManager

    // private string currentLaneAudio = "CenterLane";  // Keeps track of the current lane audio playing

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // StartCoroutine(PlayInitialLaneAudio());
        audioManager = FindObjectOfType<AudioManager>();  // Get the AudioManager reference
    }

    // Play the default lane audio after a short delay to ensure AudioManager is ready
    // IEnumerator PlayInitialLaneAudio()
    // {
    //     yield return new WaitForSeconds(0.1f);  // Small delay to ensure AudioManager is initialized
    //     FindObjectOfType<AudioManager>().PlaySound(currentLaneAudio);  // Start by playing the center lane sound
    // }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        direction.z = forwardSpeed;


        if (controller.isGrounded)
        {
            //direction.y = -1;
            //if(Input.GetKeyDown(KeyCode.UpArrow))
            if (SwipeManager.swipeUp)
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }


        //Gather the inputs on which lane we should be

        //if(Input.GetKeyDown(KeyCode.RightArrow))
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
            // UpdateLaneAudio();  // Trigger lane audio update
            UpdateLaneAudioPan();  // Update the panning instead of stopping/starting songs
        }

        //if(Input.GetKeyDown(KeyCode.LeftArrow))
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
            // UpdateLaneAudio();  // Trigger lane audio update
            UpdateLaneAudioPan();  // Update the panning instead of stopping/starting songs
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.fixedDeltaTime);
        // controller.center = controller.center;
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        // UpdateAudioFeedback();
    }

    // private void UpdateLaneAudio()
    // {
    //     // Stop the current lane audio
    //     FindObjectOfType<AudioManager>().StopSound(currentLaneAudio);

    //     // Update the audio based on the lane the player is in
    //     switch (desiredLane)
    //     {
    //         case 0: // Left lane
    //             currentLaneAudio = "LeftLane";
    //             break;
    //         case 1: // Center lane
    //             currentLaneAudio = "CenterLane";
    //             break;
    //         case 2: // Right lane
    //             currentLaneAudio = "RightLane";
    //             break;
    //     }

    //     // Play the new lane audio
    //     FindObjectOfType<AudioManager>().PlaySound(currentLaneAudio);
    // }

    // Update audio panning based on the lane
    private void UpdateLaneAudioPan()
    {
        float panValue = 0f;  // Default to center

        // Adjust panning based on the desired lane
        switch (desiredLane)
        {
            case 0:  // Left lane
                panValue = -1f;  // Left ear only
                break;
            case 1:  // Center lane
                panValue = 0f;   // Both ears
                break;
            case 2:  // Right lane
                panValue = 1f;   // Right ear only
                break;
        }

        // Call the AudioManager to update the stereo pan
        audioManager.SetLaneAudioPan(panValue);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;

        forwardSpeed += 0.1f * Time.fixedDeltaTime;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }
}
