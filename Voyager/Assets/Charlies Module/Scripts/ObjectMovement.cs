// Giovanni Orijuela, CSE 440, Fall 2018
// This is a modified version of my movement ObjectAcceleration script I made during lesson 1
// The difference here is that it stops at jerk and the Update function is replaced
// with an equivalent coroutine

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{

    // Script variables
    public float distance, velocity, acceleration, jerk; //, snap, crackle, pop;
    public float velocityDecay, accelerationDecay, jerkDecay; //, snapDecay, crackleDecay, popDecay;

    // For traveling a specified distance; if this exceeds its max distance, the object is destroyed or put back into an object pool
    public float maxDistance;

    // Bools
    public bool allowedToMove;

    private void Awake()
    {
        allowedToMove = true;
    }

    private void Start()
    {
        // Start the coroutine
        StartCoroutine(StartMoving());
    }

    // Update is called once per frame
    //void Update()
    IEnumerator StartMoving()
    {
        while (distance < maxDistance)
        {
            ChangeVelocity();
            //CheckDistance();
            gameObject.transform.Translate(Vector3.forward * velocity * Time.deltaTime);
            yield return null;
        }

        if (distance >= maxDistance)
            // Destroy game object for now; will convert to item pooling later
            Destroy(gameObject);
    }

    void ChangeVelocity()
    {
        // Code to execute when the object is allowed to move
        if (allowedToMove)
        {
            // Note: Velocity and its successive derivatives have to be multiplied by Time.deltaTime
            // Also note how everything trickles down from pop down to the displacement (distance)
            //crackle += pop * Time.deltaTime;
            //snap += crackle * Time.deltaTime;
            //jerk += snap * Time.deltaTime;
            acceleration += jerk * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            distance += velocity * Time.deltaTime;

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
        // Leave all decay values at 0 for a zero-friction environment
        else
        {
            if (velocity - velocityDecay< 0)
                velocity = 0;
            else
            {
                // Note: Velocity decay and the decay for its successive derivatives have to be multiplied by Time.deltaTime
                //crackleDecay += popDecay * Time.deltaTime;
                //snapDecay += crackleDecay * Time.deltaTime;
                //jerkDecay += snapDecay * Time.deltaTime;
                accelerationDecay += jerkDecay * Time.deltaTime;
                velocityDecay += accelerationDecay * Time.deltaTime;
                velocity -= velocityDecay * Time.deltaTime;
            }
        }
    }

    //void CheckDistance()
    //{

    //}
}
