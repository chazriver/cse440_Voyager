// Collision Damage script
// If the player bumps into an object with this script, the player takes damage and the rotation of the colliding object changes
// This is identical to the Projectile script from lesson 2 if you think about it being broken up into a movement and collision damage script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    public int damageUponHit;       // How much damage the player takes upon getting hit
    public int hitsUntilDeath;      // How many hits it takes for object to disappear

    private void OnTriggerEnter(Collider collision)
    {
        // If the player hits this object, the player takes damage
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("[CollisionDamage]: The player has hit an object and has sustained damage!");
            if(collision.gameObject.GetComponent<Shields>().HasShield == true)
            {
                collision.gameObject.GetComponent<Shields>().TakeDamage(damageUponHit);
            }
            else if(collision.gameObject.GetComponent<Shields>().HasShield == false)
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(damageUponHit);
            }
            // If this object is also an asteroid, change rotation
            if (gameObject.tag == "Asteroid") gameObject.transform.localEulerAngles = new Vector3(0, Random.Range(-180f, 180f), 0);

            // This will execute for both asteroids and projectiles
            Impact();
        }
        // If an asteroid hits this object
        if (collision.gameObject.tag == "Asteroid")
        {
            //Debug.Log("[CollisionDamage]: Collision between another asteroid detected; rotation changed!");

            collision.gameObject.GetComponent<Health>().TakeDamage(damageUponHit);

            // If this object is also an asteroid, change rotation
            if (gameObject.tag == "Asteroid") gameObject.transform.localEulerAngles = new Vector3(0, Random.Range(-180f, 180f), 0);

            Impact();
        }
        else
        {
            Debug.Log("[CollisionDamage]: Collision detected; no action taken.");
        }
    }

    // This script destroys the object if it collides a certain number of times
    // Useful for asteroids, but extra useful for projectiles
    private void Impact()
    {
        if (hitsUntilDeath - 1 <= 0) Destroy(gameObject);
        else hitsUntilDeath--;
    }
}
