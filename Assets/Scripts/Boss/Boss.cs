using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float health = 100;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        health -= 5;
        if (health < 0)
            Die();
    }

    void Die()
    {
        dead = true;
        transform.position -= new Vector3(0, 2, 0) * 6 * Time.deltaTime;
    }
}
