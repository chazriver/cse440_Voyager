// Powerup Script
// This script will manage every possible powerup in the game; powerups are broken into two main categories:
// negative and positive; negative powerups do something bad to the player (such as damage), positive powerups
// do something good to the player (such as healing)

// List of powerups so far (including descriptions):
// - Teleport (negative powerup; teleports the player to a random location)
// - Instant Damage (negative powerup; reduces player's health by a constant amount; effectively a mine or bomb)
// - Instant Health (positive powerup; increases player's health by a constant amount if already damaged)
// - Invincibility (positive powerup, proposed; negates all damage to the player for a certain amount of time)

// Note: since there will ever only be one powerup at a time, there's probably no need to use object pooling

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Enum for defining separate powerups within the same script
    public enum PowerupType { Teleporter = 0, InstantDamage, InstantHeal };

    public PowerupType powerupType;
    public float teleportRange;
    public int damageAmount;
    public int healAmount;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Debug stuff
            Debug.Log("The player has touched this powerup.");

            // Code for teleporter powerup (negative powerup)
            if (powerupType == PowerupType.Teleporter)
            {
                Vector3 telePosition = gameObject.transform.position;
                telePosition.x += Random.Range(teleportRange * -1, teleportRange);
                telePosition.y += Random.Range(teleportRange * -1, teleportRange);

                Vector3 playerRotation = collision.gameObject.transform.localEulerAngles;
                playerRotation.y += Random.Range(-180f, 180f);

                collision.gameObject.transform.rotation = Quaternion.Euler(playerRotation);
                collision.gameObject.transform.position = telePosition;
            }
            // Code for instant damage powerup (negative powerup)
            // Damage done to the player this way circumvents the health script
            else if (powerupType == PowerupType.InstantDamage)
            {
                Debug.Log("The player has been damaged!");
                
                // Call the player's TakeDamage function directly; heal amounts should be denoted using negative values
                collision.gameObject.GetComponent<Health>().TakeDamage(damageAmount);
            }
            // Code for instant heal powerup (positive powerup)
            else if (powerupType == PowerupType.InstantHeal)
            {
                // TODO: Make this actually heal the player
                Debug.Log("The player has been healed!");

                // Call the player's TakeDamage function directly; heal amounts should be denoted using negative values
                collision.gameObject.GetComponent<Health>().TakeDamage(healAmount);
            }

            // Every type of powerup should be destroyed upon being collided with the player
            Destroy(gameObject);
        }
    }
}
