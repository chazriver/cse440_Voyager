// A simple test script to see whether things colliding with the player can register a collision

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{

    private void Awake()
    {
        Debug.Log("I am awake.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            Debug.Log("The player has hit this object.");
    }
}