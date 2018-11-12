// Giovanni Orijuela, CSE 440, Fall 2018
// Modified player movement script; this one makes the player move like it's on ice
// On the topic of AddForce vs Translate, which one you want to use depends on what you want your game to feel like;
// For example, this is perfectly fine for moving a simple aircraft or spaceship along a plane, but this script is
// not suitable for making a car move on variable terrain (trust me, I tried; weird things happen... :/ )

// Note: if using this on an object that has a rigidbody AND you don't want any weird physics effects to happen,
// make sure Is Kinematic is checked in the inspector

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // For velocity and acceleration
    // Velocity is self-explanatory; acceleration is how much to increase the velocity each second
    // Leave velocity at 0 unless you want a preset velocity
    public float velocity, maxVelocity, minVelocity, acceleration;

    // For rotation related to turning
    // Leave rotationRate at 0 unless you want the player to start moving in circles immediately
    public float rotationRate, rotationIncrement;

    // For rotation related to tilting; tilting occurs when you turn, and the amount should be subtle
    public float maxTilt, tiltAmount, tiltIncrement;

    // For braking; basically a deceleration amount imposed by the player
    public float brakeRate;

    // For natural decay rate (emulates friction); assign to 0 if you want a frictionless environment
    // Basically another deceleration amount but imposed by the environment instead
    public float velocityDecayRate;

    // For rotating while moving
    // Use this to determine whether the player can rotate while standing still (velocity is zero)
    public bool rotateWhileMovingProhibited;

    // For constraining the player's Y-value (height) to a preset value
    public float yConstraint;

    // For constraining the player's X-value (height) to a preset value
    public float xMaxConstraint;
    public float xMinConstraint;

    // Object allowed to move?
    public bool allowedToMove;

    // Keycodes for directional keys
    // Use WARS for forward-left-backward-right on Colemak keyboard
    // Use WASD instead for QWERTY keyboard
    // Use F (either layout) to brake
    // Spacebar should be reserved for shooting (currently not programmed yet)
    // Jumping isn't suitable in this particular script, so none of the jump code is included
    public KeyCode moveForward,
                   moveBackward,
                   rotateCounterclockwise,
                   rotateClockwise,
                   brake;

    //public Rigidbody rb;

    // Initialize variables before first frame of game running (if any)
    private void Awake()
    {
        //gameObject.transform.position = new Vector3(0, yConstraint, 0);
        //rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // If the player isn't allowed to move, don't bother
        if (!allowedToMove) return;

        // Change velocity or rotation values
        ChangeVelocity(moveForward, moveBackward);
        ChangeRotation(rotateClockwise, rotateCounterclockwise);
        ChangeTilt(rotateClockwise, rotateCounterclockwise);
        BrakeObject(brake);
        MotionCheck();

        // How to change position of object
        // Note: if you follow how velocity was calculated, you'll find that acceleration was multiplied by Time.deltaTime
        // and here, it's being multiplied by Time.deltaTime again; this is because acceleration is distance over seconds squared
        // so to properly translate the object forward, velocity -- which was already multiplied by Time.delatTime -- has to be 
        // multiplied by Time.deltaTime again

        // How to change rotation/tilt of object
        // Quaternion used here; this applies both the tilt and rotation at once while also keeping X rotation at 0
        // How it works: any values you enter as parameters are the angles the gameObject will be set to for each axis,
        // so to apply a rotation rate, you record the object's last rotation and add the rotation rate to it

        // X value should remain zero
        // Y value is the old value plus the rotation rate (so the rotation changes over time)
        // Z is just the tilt amount; this is a value to set to, not to increment by

        gameObject.transform.rotation = Quaternion.Euler(0, gameObject.transform.localEulerAngles.y + rotationRate, tiltAmount);
        gameObject.transform.Translate(Vector3.forward * velocity * Time.deltaTime);

    }

    // Movement function
    // This is a modified version of the movement function from lesson 1
    // Changes:
    // - Movement is restricted to the forward and backward directions
    // - Pressing the forward/backward keys changes the velocity by increments of +/- acceleration
    // - Lateral movement (left/right) is changed to rotating the object instead (handled by a different script)
    // - A velocity decay mechanic is used here, like with lesson 1
    // - Jumping is not used in this script
    void ChangeVelocity(KeyCode positiveKey, KeyCode negativeKey)
    {
        // For pressing positive keys; pressing this increases the velocity by whatever the acceleration is
        // Acceleration should be applied every second, so multiply by Time.delatTime since it goes by frame
        if (Input.GetKey(positiveKey))
            velocity += acceleration * Time.deltaTime;
        // For pressing negative keys; pressing this decreases the velocity by whatever the acceleration is
        else if (Input.GetKey(negativeKey))
            velocity -= acceleration * Time.deltaTime;
        // If no key is pressed, slow down (only happens if velocityDecayRate is anything but 0)
        // Also, if you make velocityDecayRate a negative value, you'd be simulating the expansion of the universe on an exaggerated scale
        else
        {
            if (velocity > 0)
            {
                if (velocity - velocityDecayRate < 0)
                    velocity = 0;       // Ensure velocity decay doesn't overshoot into the negatives
                else
                    velocity -= velocityDecayRate * Time.deltaTime;     // Decrement otherwise
            }
            else if (velocity < 0)
            {
                if (velocity + velocityDecayRate > 0)
                    velocity = 0;       // Ensure velocity decay doesn't overshoot into the positives (if you're going backwards)
                else
                    velocity += velocityDecayRate * Time.deltaTime;     // Increment otherwise
            }
        }
    }

    // Change Rotation function
    // This changes the rotation of the object; rotations occur along the Y axis or XZ plane
    void ChangeRotation(KeyCode positiveKey, KeyCode negativeKey)
    {
        // If the object has to move in order to rotate (turn) AND the velocity is currently 0, don't bother
        if (rotateWhileMovingProhibited && velocity == 0)
            return;

        // Clockwise rotation
        // The rotation increment is how many degrees you rotate in one second, so multiply by Time.delatTime
        if (Input.GetKey(positiveKey))
        {
            //rotationRate = rotationIncrement * Time.deltaTime;
            //instead of rotating move right
            transform.Translate(Vector3.right * 5.0f * Time.deltaTime);
        }
        // Counterclockwise rotation
        else if (Input.GetKey(negativeKey))
        {
            //rotationRate = rotationIncrement * Time.deltaTime * -1;
            //instead of rotating move left
            transform.Translate(Vector3.left * 5.0f * Time.deltaTime);
        }
        else
        {
            // Reset rotation (turning) rate
            rotationRate = 0;
        }
    }

    // Change tilt function
    // For tilting the object; any rotations along the Y axis should also tilt the player sideways by some amount
    void ChangeTilt(KeyCode positiveKey, KeyCode negativeKey)
    {
        // Clockwise rotation
        if (Input.GetKey(positiveKey))
        {
            tiltAmount -= tiltIncrement * Time.deltaTime;
        }
        // Counterclockwise rotation
        else if (Input.GetKey(negativeKey))
        {
            tiltAmount += tiltIncrement * Time.deltaTime;
        }
        // Reset tilt amount
        else
        {
            if (tiltAmount > 0)
            {
                if (tiltAmount - tiltIncrement * Time.deltaTime < 0)
                    tiltAmount = 0;     // Ensure tilt amount doesn't overshoot from the zero mark
                else
                    tiltAmount -= tiltIncrement * Time.deltaTime;
            }
            else if (tiltAmount < 0)
            {
                if (tiltAmount + tiltIncrement * Time.deltaTime > 0)
                    tiltAmount = 0;     // Ensure tilt amount doesn't overshoot from the zero mark
                else
                    tiltAmount += tiltIncrement * Time.deltaTime;
            }
        }
    }

    // Brake Object function
    // For slowing an object down using braking
    void BrakeObject(KeyCode brakeKey)
    {
        if (Input.GetKey(brakeKey))
        {
            if (velocity > 0)
            {
                if (velocity - brakeRate < 0)
                    velocity = 0;       // Ensure velocity doesn't overshoot zero
                else
                    velocity -= brakeRate * Time.deltaTime;
            }
            else if (velocity < 0)
            {
                if (velocity + brakeRate > 0)
                    velocity = 0;       // Ensure velocity doesn't overshoot zero
                else
                    velocity += brakeRate * Time.deltaTime;
            }
        }
    }

    // Velocity and Rotation Check function
    // Ensures velocity and rotation doesn't exceed the min or max
    // TODO: Add a function (or modify this function) so that the player's Y position doesn't drift
    // IE, if the object should be at, say, Y = 10, then never let it stray from that position
    void MotionCheck()
    {
        // Ensure velocity does not exceed the extremes
        if (velocity > maxVelocity)
            velocity = maxVelocity;
        else if (velocity < minVelocity)
            velocity = minVelocity;

        // If the object isn't allowed to move when it's standing still, stop it from rotating
        if (velocity == 0 && rotateWhileMovingProhibited)
            rotationRate = 0;

        // Ensure tilt amount also doesn't exceed its extremes
        if (tiltAmount > maxTilt)
            tiltAmount = maxTilt;
        else if (tiltAmount < maxTilt * -1)
            tiltAmount = maxTilt * -1;

        // Ensure object's Y-value doesn't change
        // gameObject.transform version; uncomment if rigidbody does anything weird
        if (gameObject.transform.position.y != yConstraint)
        {
            Vector3 position = gameObject.transform.position;
            position.y = yConstraint;
            gameObject.transform.position = position;
        }
        //make sure object doesnt go to the left or right more than constraint
        if (gameObject.transform.position.x > xMaxConstraint)
        {
            Vector3 positionxMax = gameObject.transform.position;
            positionxMax.x = xMaxConstraint;
            gameObject.transform.position = positionxMax;
        }
        else if(gameObject.transform.position.x < xMinConstraint)
        {
            Vector3 positionxMin = gameObject.transform.position;
            positionxMin.x = xMinConstraint;
            gameObject.transform.position = positionxMin;
        }
    }

    //// If you collide with anything that isn't the ground, set velocity to zero
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag != "Ground")
    //    {
    //        velocity = 0;
    //        //allowedToMove = false;
    //    }

    //    //if (collision.gameObject.tag == "Ground")
    //    //    allowedToMove = true;
    //}
}
