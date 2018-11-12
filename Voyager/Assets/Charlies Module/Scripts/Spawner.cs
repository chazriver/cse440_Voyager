// Spawner script
// This will spawn random powerups at regular intervals (or even at random intervals if you tell it to)
// This also uses instantiation and the powerup destroys itself when used (or travels its max distance)
// Since there is only ever one powerup at a time, object pooling is probably not necessary here

// For use in levels:
// The spawner will spawn a random powerup every 20-60 seconds for the player to catch or avoid
// The spawner is signified by an arrow to make it easier to find in the Unity scene; all other spawners are like this
// Additionally, the spawner itself will be outside of where the camera is, so that spawned entities will
// appear on the screen seamlessly

// NOTE: This has been modified to spawn every possible entity, so each entity's values need to be set individually
// in the inspector because the spawner will spawn things
// TODO: Make this use object pooling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // You can use the inspector to control the values of every powerup at once
    //public Powerup.PowerupType powerupType;

    // List of powerups currently in the game; this is an array whose length is the number of
    // unique powerups currently implemented
    public GameObject[] entityList;

    // And then there's the powerup spawn interval
    public float spawnInterval;
    public bool enableSpawning;

    // Randomize positoin, velocity and sideways displacement of spawned object; if this isn't desired, then set these values to zero
    public float randomRotation;        // For setting a random rotation value to each entity
    public float randomDisplacement;    // For setting a random sideways displacement to each entity
    public float minVelocity, maxVelocity;        // For setting a random velocity

    // Actually, don't bother scaling an object; weird things happen if you do, esp. with the box collider script
    //public float minScale, maxScale;    // For setting a random scaling to each object

    // Control whether the entity is allowed to move upon spawning
    public bool allowMovementOnSpawn;

    private void Awake()
    {
    }

    private void Start()
    {
        // Note to self: don't make an infinitely looping coroutine happen in the Start function; have it happen in the coroutine
        StartCoroutine(SpawnEntity());
    }

    IEnumerator SpawnEntity()
    {
        while (enableSpawning)
        {
            // Create a new entity
            GameObject entity = entityList[Random.Range(0, entityList.Length)];

            // Enable movement on the entity
            entity.GetComponent<ObjectMovement>().allowedToMove = allowMovementOnSpawn;

            // Randomize velocity of powerup
            entity.GetComponent<ObjectMovement>().velocity = Random.Range(minVelocity, maxVelocity);

            // Randomize rotation of the powerup
            Vector3 entityRotation = gameObject.transform.localEulerAngles;
            entityRotation.y += Random.Range(-randomRotation, randomRotation);

            // Randomize sideways position of powerup
            Vector3 sidewaysOffset = gameObject.transform.TransformDirection(Vector3.right * Random.Range(-randomDisplacement, randomDisplacement));
            Vector3 entityPosition = gameObject.transform.position + sidewaysOffset;

            // Randomize entity's scaling
            //entity.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);

            // Instantiate
            Instantiate(entity, entityPosition, Quaternion.Euler(entityRotation));

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}