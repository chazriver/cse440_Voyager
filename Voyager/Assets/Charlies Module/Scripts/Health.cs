// Health script
// Modified from health script from lesson 2

// This modified health script handles damage from every possible thing in the game, such as:
// - Collisions from asteroids; asteroids are tagged as "Asteroid"; the asteroid never takes damage but hitting incurs a damage penalty on the player
// - Collisions from enemies; same as with asteroids, but hitting an enemy incurs a damage penalty on the enemy as well; enemies are tagged as "Enemies"
// - Collisions from bullets; whatever entity gets hit with a projectile gets damaged for how many hitpoints the projectile can deal; projectiles are tagged as "Projectile"
// - Getting hit by an explosion; currently not supported in this build

// As with the original health script, this also handles respawning of an entity upon entity death

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
   	public int hitPoints;
    public float timeToDie;
	private int maxHealth;
    public bool canTakeDamage;

		// Use this for initialization
	void Start ()
	{
        canTakeDamage = true;
        maxHealth = hitPoints;
	}
		
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (hitPoints <= 0)
	    {
	        StartCoroutine(Death());
	    }
	}

	IEnumerator Death()
	{
	    yield return new WaitForSeconds(timeToDie);
        //hitPoints = maxHealth;
    }

	//public GameObject Spawn
	//{
	//	get { return spawn; }
	//	set
	//	{
	//		spawn = value;
	//	}
	//}

    // For object collisions
    private void OnCollisionEnter(Collision collision)
    {
        // Theare are several combinations of collisions:
        // - Asteroid and asteroid collide -> asteroids change direction, like they bounced off one another
        // - Projectile and projectile collide -> don't bother
        // - Projectile and asteroid collide -> either don't bother or have the asteroid's direction move just a little bit
        // - Enemy and enemy collide -> should be avoided with sufficient AI, but don't bother just in case it does happen
        // - Player collides with anything -> take damage

        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "Asteroid")
        {
            Debug.Log("[Health]: Collision detected; no action taken.");
            TakeDamage(collision.gameObject.GetComponent<CollisionDamage>().damageUponHit);
        }
        else Debug.Log("[Health]: Collision detected; no action taken.");
    }

    // Function for taking damage; damage values are positive and are therefore decremented from current hitpoints
    // For healing, pass in a negative value to increase the hitpoints instead
    public void TakeDamage(int damage)
    {
        // If the entity can't take damage, don't bother
        if (!canTakeDamage) return;

        if (hitPoints - damage < 0)
        {
            hitPoints = 0;

            if (gameObject.tag == "Player")
            {
                Debug.Log("[Health]: The player has died.");
                gameObject.GetComponent<PlayerMovement>().allowedToMove = false;
                canTakeDamage = false;
            }
            else
                Destroy(gameObject);
        }
        else if (hitPoints - damage > maxHealth)
        {
            hitPoints = maxHealth;
        }
        else
        {
            Debug.Log("[Health]: Damage taken.");
            hitPoints -= damage;
        }
    }
}