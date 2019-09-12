using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private float dmg = 10.0f;

    void OnTriggerEnter(Collider other)
    {
        Player p;
        bool pHit = false;

        // If an Enemy was hit, apply damage!
        if (other.gameObject.name.Equals("PCollider")) // BANDAID, do not use name match
        {
            if (p = other.gameObject.GetComponentInChildren<Player>())
                pHit = true;
            else if (p = other.gameObject.GetComponentInParent<Player>())
                pHit = true;
            if (pHit)
            {
                //print("Damaging player for " + dmg + " points.");
                p.TakeDamage(dmg);
            }
        }
    }
}
