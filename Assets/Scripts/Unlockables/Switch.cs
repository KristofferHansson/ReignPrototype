using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    //Whether something needs to be permenantly on the switch or not
    public bool nonPermenant;
    public UnlockableWall wall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.name == "PCollider")
        {
            print("Ayy");
            wall.keys.Remove(gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "PCollider" && !nonPermenant)
        {
            wall.keys.Add(gameObject);
        }
    }
}
