using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrigger : MonoBehaviour
{
    //private float dmg = 25.0f;
    [SerializeField] private Player player;
    private float dmg;
    private int enemiesInBlade; // used to increase heat

    void Start()
    {
        dmg = player.DamagePerSecond / 100.0f;
    }

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
