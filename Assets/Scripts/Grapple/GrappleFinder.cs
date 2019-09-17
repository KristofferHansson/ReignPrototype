using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleFinder : MonoBehaviour
{

    public GameObject shortestObj;
    public GameObject player;
    Grapple grapple;
    public
    float shortestDistance = Mathf.Infinity;
    //public Renderer m_Renderer;
    //public Renderer prevRenderer = null;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerMaster");
        grapple = GetComponentInParent<Grapple>();
    }

    // Update is called once per frame
    void Update()
    {
        shortestObj = FindTarget();
        
        //print(shortestObj);
    }

    GameObject FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * 10, 10);
        float shortestDistance = Mathf.Infinity;
        Collider shortestCollider = null;
        foreach (Collider col in colliders)
        {
            if((col.tag == "PullTo" || col.tag == "Pullable" || col.tag == "BothPull" || col.tag == "BossPill")) //&& !grapple.combatMode)
            {
                if (Vector3.Distance(player.transform.position, col.transform.position) < shortestDistance)
                {
                    shortestDistance = Vector3.Distance(player.transform.position, col.transform.position);
                    shortestCollider = col;
                }
            }
            if((col.tag == "Enemy") )//&& grapple.combatMode)
            {
                
                if (Vector3.Distance(player.transform.position, col.transform.position) < shortestDistance)
                {
                    shortestDistance = Vector3.Distance(player.transform.position, col.transform.position);
                    shortestCollider = col;
                }
            }
            
        }
        if (shortestCollider == null)
            return null;
        return shortestCollider.gameObject;
    }

    
    private void OnDrawGizmos()
    {
       // Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position + transform.forward * 15, 15);
    }
}
