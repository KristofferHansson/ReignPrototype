using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrigger : MonoBehaviour
{
    //private float dmg = 25.0f;
    [SerializeField] private Player player;
    private List<Enemy> enemiesToDamage;
    private float dmg;
    private int enemiesInBlade; // used to increase heat

    void Start()
    {
        enemiesToDamage = new List<Enemy>();
        if (!player) // New blademaster has been instantiated, probably
        {
            player = GameObject.Find("PlayerMaster").GetComponent<Player>();
            //player.sawLight = GameObject.Find("sawlight").GetComponent<Light>();
        }

        dmg = player.DamagePerSecond / 100.0f;
    }

    /// Combo system version of attacking
    // void OnTriggerEnter(Collider other)
    // {
    //     Enemy e;
    //     bool eHit = false;

    //     if (other.name.Equals("ECollider"))
    //     {
    //         // If an Enemy was hit, apply damage!
    //         if (e = other.gameObject.GetComponentInChildren<Enemy>())
    //             eHit = true;
    //         else if (e = other.gameObject.GetComponentInParent<Enemy>())
    //             eHit = true;
    //         if (eHit)
    //         {
    //             enemiesToDamage.Add(e);
    //             print(enemiesToDamage.Count + " enemies in blade");
    //             //print("Damaging enemy for " + dmg + " points.");
    //             //e.TakeDamage(dmg + 30f);
    //         }
    //     }
    // }
    // void OnTriggerExit(Collider other)
    // {
    //     Enemy e;
    //     bool eHit = false;

    //     if (other.name.Equals("ECollider"))
    //     {
    //         // If an Enemy was hit, apply damage!
    //         if (e = other.gameObject.GetComponentInChildren<Enemy>())
    //             eHit = true;
    //         else if (e = other.gameObject.GetComponentInParent<Enemy>())
    //             eHit = true;
    //         if (eHit)
    //         {
    //             if (enemiesToDamage.Contains(e))
    //             {
    //                 enemiesToDamage.Remove(e);
    //                 print(enemiesToDamage.Count + " enemies in blade");
    //             }
    //         }
    //     }
    // }
    // // Multiply the saw's base damage by multiplier
    public void DealDamage(float multiplier = 1.0f) // NOTE: Used by combo attack code
    {
        int enemiesHit = 0;

        foreach (Enemy e in enemiesToDamage)
        {
            if (e is null || e == null || e.Equals(default(Enemy))) { continue; }

            enemiesHit++;
            e.KnockBack();
            e.TakeDamage((dmg + 30f) * multiplier);
        }
        if (enemiesHit > 0)
            player.IncreaseHeat();
    }

    /// Continuous damage on saw location version of attacking
    void OnTriggerEnter(Collider other)
    {
       Enemy e;
       bool eHit = false;

       if (other.name.Equals("ECollider"))
       {
           // If an Enemy was hit, apply damage!
           if (e = other.gameObject.GetComponentInChildren<Enemy>())
               eHit = true;
           else if (e = other.gameObject.GetComponentInParent<Enemy>())
               eHit = true;
           if (eHit)
           {
               e.KnockBack();
               //print("Damaging enemy for " + dmg + " points.");
               e.BeginPeriodicDamage(dmg + dmg * (player.HeatLevel / 10.0f)); // so, heat level of 3 = 30% dmg increase
               //e.TakeDamage(dmg);
           }
       }
    }
    void OnTriggerExit(Collider other)
    {
       Enemy e;
       bool eHit = false;

       if (other.name.Equals("ECollider"))
       {
           // If an Enemy was hit, apply damage!
           if (e = other.gameObject.GetComponentInChildren<Enemy>())
               eHit = true;
           else if (e = other.gameObject.GetComponentInParent<Enemy>())
               eHit = true;
           if (eHit)
           {
               e.EndPeriodicDamage();
           }
       }
    }
}
