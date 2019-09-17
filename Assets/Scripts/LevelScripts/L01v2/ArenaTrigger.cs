using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Enemy e in enemies)
            e.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("PCollider"))
        {
            foreach (Enemy e in enemies)
                e.enabled = true;
        }
    }
}
