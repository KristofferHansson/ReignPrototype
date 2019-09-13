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
    bool isGrappled = false;
    bool grappleFired = false;
    bool pullingObject = false;
    bool pullingToObject = false;
    GrappleFinder grappleFinder;
    GameObject grappledObj;
    
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.Find("PlayerMaster");
        grappleFinder = GetComponentInChildren<GrappleFinder>();
    }

    // Update is called once per frames
    void Update()
    {
        if (!isGrappled && !grappleFired && !pullingObject && !pullingToObject)
        {
            transform.position = parent.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (!isGrappled && !grappleFired)
            {
                ShootGrapple();
                //StartCoroutine(ShootGrappleProj());
            }
            else
            {
                isGrappled = false;
                grappleFired = false;
                grappleHook.transform.position = parent.transform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && !pullingObject && !pullingToObject)
        {
            if (isGrappled)
            {
                StartCoroutine(PullObjectTowards());
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && !pullingObject && !pullingToObject)
        {
            if (isGrappled)
            {
                StartCoroutine(PullToPosition());

            }
        }
        //Debug.DrawRay(transform.position, transform.up * 1000, Color.red,100);


    }


    IEnumerator PullObjectTowards()
    {
        pullingObject = true;
        while (true)
        {
            if (Mathf.Abs(Vector3.Distance(parent.transform.position, grappledObj.transform.position)) < distanceBetweenPulledObjs)
            {
                grappledObj = null;
                isGrappled = false;
                pullingObject = false;
                break;
            }

            yield return new WaitForFixedUpdate();
            //Vector3 dir = grappledPos - (Vector3)parent.transform.position;


            Vector3 dir = grappledObj.transform.position - parent.transform.position;
            grappleHook.transform.position -= dir.normalized * timePerGrappleMovement;
            grappledObj.transform.position -= dir.normalized * timePerGrappleMovement;
        }
        grappleFired = false;
        // Resets grapple
        grappleHook.transform.position = parent.transform.position;
    }
    IEnumerator PullToPosition()
    {
        pullingToObject = true;
        Vector3 grapplePos = grappleHook.transform.position;
        while (true)
        {
            grappleHook.transform.position = grapplePos;
            if (Mathf.Abs(Vector3.Distance(parent.transform.position, grappledObj.transform.position)) < distanceBetweenPulledObjs)
            {
                grappledObj = null;
                isGrappled = false;
                pullingToObject = false;
                break;
            }

            yield return new WaitForFixedUpdate();
            Vector3 dir = grappledObj.transform.position - parent.transform.position;

            parent.transform.position += dir.normalized * timePerGrappleMovement;
            //print(dir.normalized * timePerGrappleMovement);
        }
        //Resets Grapple
        grappleFired = false;
        grappleHook.transform.position = parent.transform.position;
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
}
