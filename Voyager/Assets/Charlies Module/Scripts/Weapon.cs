// Weapon script
// This is a simplified version of the weapon script from lesson 2
// Changes:
// - No magazines, reload time, or anything like that; our game doesn't need that so these changes reflect the game's weapon mechanics
// - Projectile properties are handled by the weapon, such as damage, firing rate, and maybe speed
// - The primary difference between different weapons is the projectile itself; the projectile is colored according to what fired it;
//   the shape/size of the projectile indicates what it does

// Note: In this project, the script is assigned to an empty gameobject with an arrow model
// the arrow model may be switched out for a proper turret or gun as desired

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : MonoBehaviour
{
    // This is a sub-class that stores the stats for each weapon 
    // If you want to edit every value of this sub-class within the inspector using sliders,
    // add [System.Serializable] and don't have the sub-class inherit from MonoBehaviour
    [System.Serializable]
    public class WeaponStats
    {
        // Damage the bullet can do; should have a big range; magnitudes are bigger than the highest possible health any entity
        // can ever have to account for instant damage and instant healing
        [SerializeField] [Range(-999999999, 999999999)] public int damage;

        // Time interval between shots; small values recommended for rapid fire, big values for damaging but slow shots
        [SerializeField] [Range(0.01f, 10.0f)] public float timeBetweenShots;

        // Number of hits before bullet disappears
        // A better name is "chain hit"; basically the max number of entities the bullet can go through
        [SerializeField] [Range(1, 10)] public int chainHit;

        // Max distance the bullet can travel
        [SerializeField] [Range(10, 1000)] public int maxDistance;

        // Velocity of projectile
        [SerializeField] [Range(20, 200)] public int projectileVelocity;

        // The game object corresponding to the projectile
        public GameObject projectile;
    }

    // Enum for different weapon types
    // Proposed weapons:
    // - Standard weapon; avg damage, avg fire rate, avg range, goes through 3 entities
    // - Cannon; high damage, low fire rate, low range, goes through 10 of entities
    // - Sprayer; low damage, high fire rate, high range, goes through one entity
    public enum WeaponType { Standard, Cannon, Sprayer };

    // Weapon type; this shows up as a drop-down list in the inspector
    public WeaponType weaponType;

    // The material of the projectile
    public Material projectileColor;

    // This determines whether the weapon is controlled by the player or auto-fires like a turret
    public bool controlByPlayer = false;

    // Stats for each weapon; this makes it editable in the inspector
    public WeaponStats[] weapons = new WeaponStats[System.Enum.GetValues(typeof(WeaponType)).Length];

    // Should the turret fire?
    public bool allowedToFire = true;

    // The button for firing
    public KeyCode fireWeapon = KeyCode.Space;

    // Bool for determining whether the weapon was fired by the player; prevents the player from damaging self
    // Actually, whoever heard of a game where bullets affected everyone but the player? Will implement this
    // if it makes gameplay too difficult
    //public bool firedByPlayer = true;

    private void Awake()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(WeaponType)).Length; i++)
        {
            weapons[i].projectile.GetComponent<CollisionDamage>().damageUponHit = weapons[i].damage;
            weapons[i].projectile.GetComponent<CollisionDamage>().hitsUntilDeath = weapons[i].chainHit;

            weapons[i].projectile.GetComponent<ObjectMovement>().maxDistance = weapons[i].maxDistance;
            weapons[i].projectile.GetComponent<ObjectMovement>().velocity = weapons[i].projectileVelocity;
            weapons[i].projectile.GetComponent<ObjectMovement>().allowedToMove = true;

            weapons[i].projectile.GetComponent<MeshRenderer>().material = projectileColor;
        }
    }

    // If this is a turret, start firing when the game starts
    private void Start()
    {
        if (controlByPlayer) StartCoroutine(FireAsPlayerControlledWeapon());
        else StartCoroutine(FireAsTurret());
    }

    IEnumerator FireAsTurret()
    {
        while (allowedToFire)
        {
            Debug.Log("[Weapon]: Firing weapon...");
            float timeBetweenShots = weapons[(int)weaponType].timeBetweenShots;
            Instantiate(weapons[(int)weaponType].projectile, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    IEnumerator FireAsPlayerControlledWeapon()
    {
        while (allowedToFire)
        {
            float timeBetweenShots = weapons[(int)weaponType].timeBetweenShots;
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("[Weapon]: Firing weapon...");
                //float timeBetweenShots = weapons[(int)weaponType].timeBetweenShots;
                Instantiate(weapons[(int)weaponType].projectile, gameObject.transform.position, gameObject.transform.rotation);
            }
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
