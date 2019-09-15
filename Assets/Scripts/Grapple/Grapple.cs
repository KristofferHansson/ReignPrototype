using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    //Speed of pulling and moving towards objects
    [Tooltip("Speed of pulling and moving towards objects")]
    [Range(.2f, .8f)]
    public float timePerGrappleMovement = .05f;

    [Tooltip("Determines if player can shoot bad guys or not")]
    public bool combatMode = false;
    [Tooltip("Distance between player and object after pulled")]
    public float distanceBetweenPulledObjs;
    public GameObject grappleHook;
    public GameObject target;
    public GameObject parent;
    public bool isGrappled = false;
    bool grappleFired = false;
    public bool pullingObject = false;
    public bool pullingToObject = false;
    GrappleFinder grappleFinder;
    public GameObject grappledObj;
    public GameObject pCollider;
    public UIMiddleman uiMiddleMan;
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("PlayerMaster");
        pCollider = GameObject.Find("PCollider");
        grappleFinder = GetComponentInChildren<GrappleFinder>();
    }

    // Update is called once per frames
    void Update()
    {
        uiMiddleMan.SetGrappleMode(combatMode);
        if(!isGrappled)
        {
            grappleFired = false;
            pullingObject = false;
            pullingToObject = false;
            grappledObj = null;
        }
        if (!isGrappled && !grappleFired && !pullingObject && !pullingToObject)
        {
            transform.position = pCollider.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
            combatMode = !combatMode;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (!isGrappled && !pullingObject && !pullingToObject)
            {
                ShootGrapple();
                //StartCoroutine(ShootGrappleProj());
            }
            else if(!pullingToObject && pullingObject)
            {
                isGrappled = false;
                grappleFired = false;
                grappledObj = null;
                grappleHook.transform.position = pCollider.transform.position;
            }
            else
            {
                isGrappled = false;
                grappleFired = false;
                grappledObj = null;
                grappleHook.transform.position = pCollider.transform.position;
            }
        }
        if(grappledObj != null)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !pullingObject && !pullingToObject && grappledObj.tag != "PullTo")
            {
                if (isGrappled)
                {
                    StartCoroutine(PullObjectTowards());
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && !pullingObject && !pullingToObject && grappledObj.tag != "Pullable")
            {
                if (isGrappled)
                {
                    StartCoroutine(PullToPosition());

                }
            }
        }
        
        //Debug.DrawRay(transform.position, transform.up * 1000, Color.red,100);


    }


    IEnumerator PullObjectTowards()
    {
        Vector3 offset = new Vector3(0, 2, 0);
        pullingObject = true;
        while (true)
        {
            //If pull get's interrupted
            if (!isGrappled)
                break;
            if (grappledObj == null)
                break;
            if (Mathf.Abs(Vector3.Distance(parent.transform.position + offset, grappledObj.transform.position)) < distanceBetweenPulledObjs)
            {
                grappledObj = null;
                isGrappled = false;
                pullingObject = false;
                break;
            }

            yield return new WaitForFixedUpdate();
            //Vector3 dir = grappledPos - (Vector3)parent.transform.position;

            if (grappledObj == null)
                break;
            Vector3 dir = grappledObj.transform.position - (parent.transform.position+ offset);
            grappleHook.transform.position -= dir.normalized * timePerGrappleMovement;
            grappledObj.transform.position -= dir.normalized * timePerGrappleMovement;
        }
        grappleFired = false;
        // Resets grapple
        grappleHook.transform.position = pCollider.transform.position;
    }
    IEnumerator PullToPosition()
    {
        pullingToObject = true;
        Vector3 grapplePos = grappleHook.transform.position;

        while (true)
        {
            //If pull get's interrupted
            if (!isGrappled)
                break;
            grappleHook.transform.position = grapplePos;
            if (Mathf.Abs(Vector3.Distance(parent.transform.position, grappledObj.transform.position)) < distanceBetweenPulledObjs)
            {
                grappledObj = null;
                isGrappled = false;
                pullingToObject = false;
                break;
            }

            yield return new WaitForFixedUpdate();

            if (grappledObj == null)
                break;
            Vector3 dir = grappledObj.transform.position - parent.transform.position;

            parent.transform.position += dir.normalized * timePerGrappleMovement;
            //print(dir.normalized * timePerGrappleMovement);
        }
        //Resets Grapple
        grappleFired = false;
        grappleHook.transform.position = pCollider.transform.position;
    }

   
    void ShootGrapple()
    {
        if (grappleFinder.shortestObj == null)
        {
            return;
        }
        else
        {
            grappleFired = true;
            grappledObj = grappleFinder.shortestObj;
            grappleHook.transform.position = grappledObj.transform.position;
            isGrappled = true;
            if (combatMode)
            {
                //Do bad things to enemy here
            }
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "floor")
        {
            isGrappled = false;
        }
            
    }
    
}
