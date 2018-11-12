using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour {

    public int shieldHitPoints;
    private int maxShield;
    public bool hasShield;
    private float rechargeDelay;
    private bool currentlyRecharging;


    // Use this for initialization
    void Start()
    {
        currentlyRecharging = false;
        shieldHitPoints = 100;
        maxShield = shieldHitPoints;
        rechargeDelay = 10.0f;
    }
    private void Update()
    {
        if(shieldHitPoints < maxShield && currentlyRecharging == false)
        {
            currentlyRecharging = true;
            StartCoroutine(ShieldRechargeDelay(rechargeDelay));
        }
    }
    IEnumerator ShieldRechargeDelay(float rechargeDelay)
    {
        Debug.Log(" shields starting to recharge");
        yield return new WaitForSeconds(rechargeDelay);
        StartCoroutine(Recharge());
    }
    IEnumerator Recharge()
    {
        Debug.Log(" shields recharged");
        yield return new WaitForSeconds(0.1f);
        shieldHitPoints = maxShield;
        currentlyRecharging = false;
    }
    public void TakeDamage(int damage)
    {
        // If the entity can't take damage, don't bother
        if (hasShield == false) return;

        if (shieldHitPoints - damage < 0)
        {
            shieldHitPoints = 0;
            hasShield = false;
        }
        else if (shieldHitPoints - damage > maxShield)
        {
            shieldHitPoints = maxShield;
        }
        else
        {
            Debug.Log("[Shield]: Damage taken.");
            shieldHitPoints -= damage;
        }
    }
    public int ShieldHitPoints
    {
        get { return shieldHitPoints; }
        set
        {
            shieldHitPoints += value;
            if (shieldHitPoints > maxShield)
            {
                shieldHitPoints = maxShield;
            }
        }
    }
    public bool HasShield
    {
        get { return hasShield;}
        set
        {
            hasShield = value;
        }
    }
}
