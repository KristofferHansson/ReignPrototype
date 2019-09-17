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
        if (switchVar.eventStarted)
            timer -= 1 * Time.deltaTime;
        if (switchVar.eventStarted&& transform.position.y <100)
        {
            transform.position += new Vector3(0, fogSpeed, 0) * Time.deltaTime;
        }
            
    }

    
}
