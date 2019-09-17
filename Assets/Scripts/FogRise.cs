using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogRise : MonoBehaviour
{
    public float fogSpeed;
    public Switch switchVar;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(transform.position.y);
        if (switchVar.eventStarted)
            timer -= 1 * Time.deltaTime;
        if (timer < 0 && transform.position.y < 20)
            transform.position += new Vector3(0, fogSpeed, 0) *Time.deltaTime;
    }

    
}
