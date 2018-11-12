// Giovanni Orijuela, CSE 440, Fall 2018
// This is a modified version of my movement script from lesson 1
// The difference here is that this accounts for jerk, snap, crackle, and pop, the 3rd, 4th, 5th, and 6th derivatives of the displacement function
// Basically, not only can the rate at which an object speeds up also speed up, but that rate can also speed up (if that makes sense)
// This script will make an object speed up until it travels a certain distance

// Physics lesson:
// Tracking an object's position over time will give you the displacement function
// Deriving that gives you the velocity function and deriving that again gives you the acceleration function

// Here's a table listing out successive derivations, starting at the displacement function
// +---------------------------+-------------------------+
// | DERIVING THIS FUNCTION... | GIVES YOU THIS FUNCTION |
// +---------------------------+-------------------------+
// | Displacement              | Velocity                |
// +---------------------------+-------------------------+
// | Velocity                  | Acceleration            |
// +---------------------------+-------------------------+
// | Acceleration              | Jerk                    |
// +---------------------------+-------------------------+
// | Jerk                      | Snap or Jounce          |
// +---------------------------+-------------------------+
// | Snap                      | Crackle                 |
// +---------------------------+-------------------------+
// | Crackle                   | Pop                     |
// +---------------------------+-------------------------+

// An object traveling at a constant speed is boring; usually, an object starts at zero velocity and speeds up; this is acceleration
// Usually, the rate at which an object speeds up is constant; in other words, acceleration is constant
// If this is the case, you want to leave distance and velocity set to zero (unless you want to have an initialized nonzero velocity)

// If the rate at which an object speeds up is NOT constant (IE, acceleration is not constant), the rate at which the acceleration changes
// is called jerk; if you've ever been in a car and you've been thrown back in your seat, that's jerk
// You can keep going, too; after jerk is snap, then crackle, then pop; the G-forces in a vehicle with a constant pop would be insane (and probably lethal)

// Note: if all you care about is acceleration, you can leave everything after acceleration set to zero
// This also applies to the decay values; however, assigning values to pop decay will result in an extremely sudden stop,
// so I'd recommend assigning an acceleration decay and leaving the rest zero

// Also note: for most games, going all the way to the 6th derivative is overkill; stopping at acceleration or even jerk is plenty already
// This is just a proof of concept that it can go all the way to pop and successive projects may have the extra stuff commented out

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAcceleration : MonoBehaviour {

    // Script variables
    public float distance, velocity, acceleration, jerk, snap, crackle, pop;
    public float velocityDecay, accelerationDecay, jerkDecay, snapDecay, crackleDecay, popDecay;
    public float maxDistance;

    // This multiplies the velocity...pop by some factor; this becomes the decay values
    public float degredationFactor;

    // Bools
    public bool allowedToMove;

    private void Awake()
    {
        allowedToMove = true;
    }

    private void Start()
    {
        // Start the 
    }

    // Update is called once per frame
    // TODO: add a delay so the object doesn't start moving until a certain amount of time passes
    void Update ()
    {
        // Code to execute when the object is allowed to move
        if (allowedToMove)
        {
            // Note: Velocity and its successive derivatives have to be multiplied by Time.deltaTime
            // Also note how everything trickles down from pop down to the displacement (distance)
            crackle      += pop          * Time.deltaTime;
            snap         += crackle      * Time.deltaTime;
            jerk         += snap         * Time.deltaTime;
            acceleration += jerk         * Time.deltaTime;
            velocity     += acceleration * Time.deltaTime;

            // Code for the object to stop moving
            // Here, it's set to stop moving when it hits a target DISTANCE
            //if (velocity > maxVelocity)
            //{
            //    allowedToMove = false;
            //    velocity = maxVelocity;
            //}
            if (distance >= maxDistance)
            {
                allowedToMove = false;
            }
        }
        // Code for the object to slow down
        // Note: decays are denoted using positive numbers, so subtract that amount from the velocity
        else
        {
            if (velocity - velocityDecay < 0)
                velocity = 0;
            else
            {
                // Note: Velocity decay and and the decay for its successive derivatives have to be multiplied by Time.deltaTime
                crackleDecay      += popDecay          * Time.deltaTime;
                snapDecay         += crackleDecay      * Time.deltaTime;
                jerkDecay         += snapDecay         * Time.deltaTime;
                accelerationDecay += jerkDecay         * Time.deltaTime;
                velocityDecay     += accelerationDecay * Time.deltaTime;

                velocity -= velocityDecay * Time.deltaTime;
            }
        }

        // For objects made in Blender and imported into Unity, move along the UP/DOWN axis instead
        // Otherwise, move along the FORWARD/BACKWARD axis
        // TODO: Add code to account for this discrepancy and have it be a checkmark in the inspector
        // For example, if (madeInBlender) "go up"; else "go forward"

        // Just in case this vector business is still confusing, try hovering the cursor over the word "up"
        // Notice how it's a shorthand for (0,1,0), the unit "up" vector; if it weren't for the other variables,
        // this object would be moving one unit "up" every frame, and remember that movement in the positive Y axis 
        // corresponds to moving "up" (since this is a Blender-made object, it's moving up; otherwise, it would
        // be Vector3.forward; this is why I've been writing quotes around up)

        // When you multiply by the velocity, you're effectively scaling up/down that "up" movment accordingly
        // to how much the object should move; Time.deltaTime ensures that the movement isn't affected by the
        // computer's framerate
        gameObject.transform.Translate(Vector3.forward * velocity * Time.deltaTime);

        // To track the distance the object has traveled
        distance += velocity * Time.deltaTime;
    }
}
