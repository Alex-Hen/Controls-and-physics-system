using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
// Setup the library that lets us use the input system

public class PlayerMovement : MonoBehaviour
{
    // Set up some variables for later use
    Vector3 direction;
    [SerializeField] float multiplier = 1;
    [SerializeField] float jumpHeight = 1;
    bool onGround = true;

    // To get proper collisions, create our player under the rigidbody
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        // Set the initial velocity to 0
        direction = Vector3.zero;

        // Properly assign the rigid body
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // To begin, start moving by updating our motion based on the direction and a mutliplier - scaled properly
        // transform.position += direction * multiplier * Time.deltaTime;

        // Better mothod - update the position based on forces - allows for more physics
        rb.AddForce(direction * multiplier);

        // Just so that we don't have infinite speed, only increase add force if we are below a certain velocity
        // Use rb.velocity.magnitude to get the magnitude of the velocity

    }


    // Add some other functions that read input ---------------------------------------------

    // When we try and move around horizontally
    void OnMove(InputValue value)
    {
        // Get the 2D vector input
        Vector2 input = value.Get<Vector2>();


        // Split the input into its components - x and y
        float moveX = input.x;
        float moveZ = input.y;

        // Directly update our position to move around
        // transform.position += new Vector3(moveX, 0, moveZ);

        // Instead of directly updating the position, let's update the direction that we want to move in
        // direction = new Vector3(moveX, 0, moveZ);

        // To include collision detection with updating the direction, apply the direction based on rigidbody
        // rb.velocity = new Vector3(moveX, 0, moveZ);

        // Since the update function is now handling adding all forces to our player, we just need to set the direction
        direction = new Vector3(moveX, 0, moveZ);


        // Print out the input to see what's happening here
        // Debug.Log(input);
    }


    // When we try and jump
    void OnJump(InputValue value)
    {
        Debug.Log("Are we on the Ground: " + onGround);

        // This is jump an input, no need to split into components

        // If we detect a jump push, update our vertical position by 1
        // transform.position += new Vector3(0, 1, 0);



        // If we are not on the groudn - do not jump
        if (onGround == false)
        {
            // Immediately exit the function
            return;
        }

        // Otherwise, jump
        // Since we now use a force-based movement system, jump by adding some vertical force
        Vector3 jumpForce = new Vector3(0, 300, 0);
        rb.AddForce(jumpForce * jumpHeight);


        // Print out the action so that we know when we try to jump
        // Debug.Log("Jumping");
    }


    // Conditional functions ---------------------------

    // Check to see if we are on the ground - if so, then set the onGround to true
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter - # of contacts: " + collision.contactCount);

        // Go through the amount of contacts
        for (int i = 0; i < collision.contactCount; i++)
        {
            // Get the contact surface
            ContactPoint c = collision.contacts[i];

            // Calcuate the dot product between the up and the contact surface
            float ratioInDirection = Vector3.Dot(Vector3.up, c.normal);
            Debug.Log("Contact Ratio: " + ratioInDirection);

            // If it's above a certain ratio, we are on the ground
            if (ratioInDirection > 0.8)
            {
                onGround = true;
                Debug.Log("On Ground");
            }
        }
    }


    // Check to see if we are off of the ground - if so, then set the onGround to false 
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision Exit - # of contacts: " + collision.contactCount);

        // If we are not contacting anything, we are off of the ground
        // This setup should allow for wall jumps
        if (collision.contactCount == 0)
        {
            onGround = false;
            Debug.Log("Off Ground");
        }

    }


}

// Need to check OnCollisionState
