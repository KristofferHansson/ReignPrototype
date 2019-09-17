using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPill : MonoBehaviour
{

    Boss boss;
    public Grapple grapple;
    Rigidbody rb;
    float timer = .7f;
    public bool fall = false;
    public bool isFirst;

    //Only need this for the first pill
    public LevelStarter levelStarter;
    // Start is called before the first frame update
    void Start()
    {
        grapple = GameObject.Find("Grapple").GetComponent<Grapple>();
        rb = GetComponent<Rigidbody>();
        boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if(grapple.grappledObj == gameObject && grapple.pullingObject)
        {
            fall = true;
        }
        Die();
    }

    void Die()
    {
        if(fall)
        {
            timer -=  Time.deltaTime;
            if(timer < 0)
            {
                
                //Meaning The first pill to go
                if (isFirst)
                {
                    levelStarter.firstPluck = true;
                    
                    
                }
                boss.TakeDamage();
                Destroy(gameObject);
            }
            
        }
        
    }

    
}
