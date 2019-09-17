﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPill : MonoBehaviour
{
    public Grapple grapple;
    Rigidbody rb;
    float timer = .8f;
    public bool fall = false;
    
    // Start is called before the first frame update
    void Start()
    {
        grapple = GameObject.Find("Grapple").GetComponent<Grapple>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(grapple.grappledObj == gameObject)
        {
            print("Ay");
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
                rb.useGravity = true;
                grapple.grappledObj = null;
                grapple.isGrappled = false;
                grapple.pullingObject = false;
            }
            
        }
        
    }

    
}