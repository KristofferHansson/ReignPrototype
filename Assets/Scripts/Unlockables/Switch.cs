﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    //Whether something needs to be permenantly on the switch or not
    public bool nonPermenant;
    public UnlockableWall wall;
    public CameraShake cameraShake;
    public bool runEvent;
    public bool eventStarted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void StartEvent()
    {
        eventStarted = true;
        cameraShake.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.name == "PCollider" )
        {
            if (runEvent)
                StartEvent();
            wall.isUnlocked = true;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BothPull" || other.tag == "Pullable")
        {
            wall.isUnlocked = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (wall == null)
            return;
        if (other.name == "PCollider" && !nonPermenant)
        {
            wall.isUnlocked = false;
        }
        if ((other.tag == "BothPull" || other.tag == "Pullable") && !nonPermenant)
        {
            wall.isUnlocked = false;
        }
    }
}
