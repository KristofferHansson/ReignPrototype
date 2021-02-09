using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float move_Speed = 1.0f;
    [SerializeField] private float maxGrappleDistance = 30.0f;
    [SerializeField] private GameObject playerMaster;
    [SerializeField] private GameObject weaponHinge;
    [SerializeField] private GameObject weaponColliderForRagdoll = null;
    [SerializeField] private GameObject blade;
    [SerializeField] private UIMiddleman ui;
    [SerializeField] private Transform camRig;
    [SerializeField] private SawTrigger saw;

    private Player player;
    private Rigidbody m_Rigidbody;
    private Vector3 move;
    private GameObject heldObject;
    private Enemy heldEnemy;
    private bool bladeExtended = false;
    private bool bladeRetracting = false;
    private bool bladeGrappling = false;
    private Vector3 bladeDefaultPos;

    // Start is called before the first frame update
    void Start()
    {
        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();

        player = playerMaster.GetComponent<Player>();
        m_Rigidbody = GetComponent<Rigidbody>();
        bladeDefaultPos = blade.transform.localPosition;
        //attackTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /// KILLZ
        if (transform.position.y < -40.0f) // Eventually change to get current level killz
            player.TakeDamage(100.0f);

        float x = 0.0f, z = 0.0f;

        /// MOVEMENT
        // Check for input // Convert to getaxes
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            z += 1.0f;
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            z += -1.0f;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            x += -1.0f;
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            x += 1.0f;

        // Update movement vector
        move.x = x;
        move.z = z;
        move.Normalize();
        move = Quaternion.Euler(0f, -45f, 0f) * move; // adjust rot for camera view
        move *= move_Speed;
        //move.y = Physics.gravity.y * Time.deltaTime * 2.0f;
        if (move.y < -3.0f)
            move.y = -3.0f;
        else if (move.y > 3.0f)
            move.y = 3.0f;

        Move();


        /// AIM, ROTATION, AND GRAPPLING
        
        // Saw direction and grappling / pulling stuff
        // Raycast from camera, simuilate ground plane to find intersection point
        Vector3 mousePos = Input.mousePosition;
        //print(mouse);
        Plane aimZPlane = new Plane(Vector3.up, camRig.transform.position + new Vector3(0f,blade.transform.position.y,0f));
        Ray aimRay = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0f));
        float distToAimZPlane = 0f;
        aimZPlane.Raycast(aimRay, out distToAimZPlane);
        Debug.DrawRay(aimRay.origin, aimRay.direction * distToAimZPlane, Color.cyan);
        mousePos = aimRay.GetPoint(distToAimZPlane);
        //print(mouse);
        Vector3 origin = m_Rigidbody.transform.position;
        origin.y = mousePos.y;
        Vector3 mouseDirection = mousePos - origin;
        mouseDirection.y = 0;
        
        // Rotate saw
        if (!bladeGrappling)
            weaponHinge.transform.forward = mouseDirection;

        // Grapple stuff
        // FUTURE: conditional block: if (grapple is enabled)
        {
            // If object is held and release button pressed (obj can be held w/ blade in or out)
            if (heldObject && Input.GetMouseButtonDown(1))
            {// drop object
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.transform.parent = null;
                heldObject = null;

                if (heldEnemy != null)
                    heldEnemy.Release();
            }
            // If blade is out (extended)
            else if (bladeExtended)
            {
                // scroll wheel reels in blade
                if (Input.mouseScrollDelta.y < 0.0f)
                {
                    if (!bladeRetracting && heldEnemy != null) // Heal when enemies are pulled in
                        player.Heal(10.0f);
                    StartCoroutine("RetractBladeOverTime");
                }
            }
            else if (bladeGrappling)
            {
                // disable grapple indicator text and dot
                ui.ShowGrappleIndicator(-1.0f);
            }
            // Blade is in default position
            else
            {
                // NOTE: Two raycasts used so that player can grapple to object behind enemy or pull-to obj that would otherwise be first obj hit
                // 'grapple' ray cast each frame to display indicator, 'pull-to' ray cast only on frames where pull-to command detected

                // Send out ray each frame to update grapple indicator on UI
                origin.y = m_Rigidbody.transform.position.y + 1.5f;
                Debug.DrawRay(origin, mouseDirection * 100.0f, Color.yellow);
                bool objHit = false;
                if (objHit = Physics.Raycast(origin, mouseDirection, out RaycastHit hitObj, maxGrappleDistance, ~((1 << 2) | (1 << 9))) && hitObj.collider.gameObject.name != "sawtrigger" && hitObj.distance > 6f)
                {
                    // show green indicator with distance of object
                    ui.ShowGrappleIndicator(hitObj.distance);
                    // update gpi pos
                    ui.UpdateGrappleIndicatorLocation(hitObj.collider.gameObject.transform.position);
                }
                else
                {
                    // disable grapple indicator text and dot
                    ui.ShowGrappleIndicator(-1.0f);
                }

                // GRAPPLE-TO movement command
                if (objHit && (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space)) && hitObj.distance > 6f)
                {
                    // For instantaneous movement to grapple point:
                    //playerMaster.transform.position = playerMaster.transform.position + blade.transform.forward * hitObj.distance;
                    
                    // For gradual movement to grapple point:
                    bladeGrappling = true;
                    Vector3 targetPos = playerMaster.transform.position + blade.transform.forward * hitObj.distance;
                    float bladeDistance = hitObj.distance - 2.0f;
                    blade.transform.position += blade.transform.forward * bladeDistance;
                    blade.transform.parent = null;
                    StartCoroutine("MoveToLocationOverTime", targetPos);
                }
                // PULL OBJECT/ENEMY command
                else if (Input.GetMouseButtonDown(1)
                    && Physics.Raycast(origin, mouseDirection, out RaycastHit hit, maxGrappleDistance, ~(1 << 2)) && hit.collider.gameObject.name != "sawtrigger" && hit.distance > 6f)
                {
                    GameObject goHit = hit.collider.gameObject;
                    //print("OBJECT HIT! " + goHit.name);
                    FindAndReturnComponent(goHit, out Rigidbody rb);
                    if (rb != null) // only pull objects with a rigidbody to avoid static objs
                    {
                        //print("PULL");

                        FindAndReturnComponent(goHit, out Enemy e);
                        if (e != null)
                        {
                            e.Grab();
                            heldEnemy = e;
                        }

                        // Extend blade
                        bladeExtended = true;
                        float bladeDistance = hit.distance - 5.0f;
                        blade.transform.position += blade.transform.forward * bladeDistance;
                        //StartCoroutine("RetractBladeOverTime");

                        // Begin holding object
                        rb.isKinematic = true;
                        rb.gameObject.transform.parent = blade.transform;
                        heldObject = rb.gameObject;
                    }
                }
            }
        }
    }

    public void Die()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        Camera.main.transform.parent = null;
        this.enabled = false;

        if (weaponColliderForRagdoll != null) {
            weaponHinge.transform.parent = null;
            weaponColliderForRagdoll.SetActive(true);
            Rigidbody tempRb = weaponHinge.gameObject.AddComponent<Rigidbody>();
            tempRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public GameObject GetWeaponHinge()
    {
        return weaponHinge;
    }

    // Find component in target game object or parents
    private void FindAndReturnComponent<T>(GameObject objToSearch, out T ret)
    {
        T c = objToSearch.GetComponent<T>();
        if (c == null || c.Equals(default(T)))
        {
            c = objToSearch.GetComponentInParent<T>();
        }
        ret = c;
    }

    private void Move()
    {
        m_Rigidbody.velocity = new Vector3(move.x, m_Rigidbody.velocity.y, move.z);
        //print(m_Rigidbody.velocity);
    }

    // Move player towards target location over time
    private IEnumerator MoveToLocationOverTime(Vector3 targetPos)
    {
        float grappleToSpeed = 7f;
        m_Rigidbody.useGravity = false;

        while (true)
        {
            // NOTE: This math creates a decelerating effect as player nears target
            playerMaster.transform.position += (targetPos - playerMaster.transform.position) * Time.deltaTime * grappleToSpeed;
            
            if (Vector3.Distance(playerMaster.transform.position, targetPos) <= 2f) {
                //print("Player at target position. Stopping grapple-to.");
                blade.transform.parent = weaponHinge.transform;
                blade.transform.localPosition = bladeDefaultPos;
                m_Rigidbody.useGravity = true;
                bladeGrappling = false;
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    // EXPERIMENTAL: Move object to player over time
    private IEnumerator PullObjectOverTime(GameObject targetObj)
    {
        float grappleToSpeed = 7f;
        while (true)
        {
            targetObj.transform.position += (targetObj.transform.position - playerMaster.transform.position) * Time.deltaTime * grappleToSpeed;
            
            if (Vector3.Distance(playerMaster.transform.position, targetObj.transform.position) <= 2f) {
                print("Object at player position. Stopping pull-to.");
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    // Retract blade over time
    private IEnumerator RetractBladeOverTime()
    {
        float grappleToSpeed = 40f;
        bladeRetracting = true;
        while (true)
        {
            blade.transform.localPosition += new Vector3(0f,0f,-1f) * Time.deltaTime * grappleToSpeed;
            
            if (blade.transform.localPosition.z < bladeDefaultPos.z) {
                blade.transform.localPosition = bladeDefaultPos;
                bladeExtended = false;
                bladeRetracting = false;
                //print("Blade retracted. Stopping pull-to.");
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
