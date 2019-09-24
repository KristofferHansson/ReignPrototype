using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float move_Speed = 1.0f;
    [SerializeField] private GameObject playerMaster;
    [SerializeField] private GameObject weaponHinge;
    [SerializeField] private GameObject blade;
    [SerializeField] private UIMiddleman ui;
    [SerializeField] private Transform camRig;
    [SerializeField] private SawTrigger saw;
    [SerializeField] private Animator sawAnims;

    public Grapple grapple;
    private Player player;
    private Rigidbody m_Rigidbody;
    private Vector3 move;
    private GameObject heldObject;
    private Enemy heldEnemy;
    private bool bladeExtended = false;
    private bool firstHitActivated = false;
    private bool secondHitActivated = false;
    private bool thirdHitActivated = false;
    private float bladeDistance = 0.0f;
    private float timeOfFirstAttack = 0.0f;
    private float timeOfLastAttack = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();

        player = playerMaster.GetComponent<Player>();
        m_Rigidbody = GetComponent<Rigidbody>();

        //attackTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -40.0f) // Eventually change to get current level killz
            player.TakeDamage(100.0f);

        float x = 0.0f, z = 0.0f;

        // Check for input // Convert to getaxes
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            z += 1.0f;
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            z += -1.0f;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            x += -1.0f;
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            x += 1.0f;

        /// Attacking
        float diff = Time.time - timeOfLastAttack;
        if (firstHitActivated && diff > 1.1f) // reset attack timer
        {
            print("resetting attack timers");
            firstHitActivated = secondHitActivated = thirdHitActivated = false;
        }
        if (!firstHitActivated && Input.GetMouseButtonDown(1))
        {
            print("first combo hit");
            Attack1();
        }
        else if (!secondHitActivated && diff < 0.66f && Input.GetMouseButtonDown(1))
        {
            print("second combo hit");
            Attack2();
        }
        else if (!thirdHitActivated && diff < 0.66f && Input.GetMouseButtonDown(1))
        {
            print("third combo hit");
            Attack3();
        }

        // Update movement vector
        move.x = x;
        move.z = z;
        move.Normalize();
        move = Quaternion.Euler(0f, -45f, 0f) * move;
        move *= move_Speed;
        //move.y = Physics.gravity.y * Time.deltaTime * 2.0f;
        if (move.y < -3.0f)
            move.y = -3.0f;
        else if (move.y > 3.0f)
            move.y = 3.0f;

        Move();

        //if (!attacking)
        {
            // Saw direction and grappling / pulling stuff
            // get mousepos
            Vector3 mouse = Input.mousePosition;
            //print(mouse);
            mouse = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, (Camera.main.transform.position - camRig.position).magnitude - 1f));
            Vector3 temp = m_Rigidbody.transform.position;
            temp.y = 0;
            mouse.y = 0;
            Vector3 mouseDiff = mouse - temp;
            //print(mouse);

            // rotate saw
            weaponHinge.transform.forward = mouseDiff;
        }

        {
            /* Commenting Out Grapple Code***********************************
            if (heldObject && Input.GetMouseButtonDown(1))
            {// drop object if held
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.transform.parent = null;
                heldObject = null;

                if (heldEnemy != null)
                    heldEnemy.Release();
            }
            else
            {
                if (!bladeExtended)
                {
                    // Note: hitObj is for grappling self, hitEnemy is for pulling enemy
                    // Send out ray each frame to update grapple indicator on UI
                    temp.y = m_Rigidbody.transform.position.y + 1.5f;
                    bool objHit = false;
                    if (objHit = Physics.Raycast(temp, mouseDiff, out RaycastHit hitObj, 30.0f, ~((1 << 2) | (1 << 9))) && hitObj.collider.gameObject.name != "sawtrigger")
                    {
                        // show green indicator with distance of object
                        ui.ShowGrappleIndicator(hitObj.distance);
                    }
                    else
                    {
                        ui.ShowGrappleIndicator(-1.0f);
                    }

                    Debug.DrawRay(temp, mouseDiff * 100.0f, Color.yellow);
                    if (objHit && Input.GetMouseButtonDown(0)) // GRAPPLE movement command
                    {
                        playerMaster.transform.position = playerMaster.transform.position + blade.transform.forward * hitObj.distance;
                    }
                    else if (Input.GetMouseButtonDown(1)) // PULL command
                    {
                        //print("PULL");
                        // lift vector off ground
                        Debug.DrawRay(temp, mouseDiff * 100.0f, Color.yellow);
                        if (Physics.Raycast(temp, mouseDiff, out RaycastHit hit, 100.0f, ~(1 << 2)) && hit.collider.gameObject.name != "sawtrigger")
                        {
                            GameObject goHit = hit.collider.gameObject;
                            //print("OBJECT HIT! " + goHit.name);
                            FindAndReturnComponent(goHit, out Rigidbody rb);
                            if (rb != null) // only pull objects with a rigidbody to avoid static objs
                            {
                                FindAndReturnComponent(goHit, out Enemy e);
                                if (e != null)
                                {
                                    e.Grab();
                                    heldEnemy = e;
                                }

                                // Extend blade
                                bladeExtended = true;
                                //blade.transform.position = hit.point;
                                bladeDistance = hit.distance - 5.0f;
                                blade.transform.position += blade.transform.forward * bladeDistance;

                                // Hold object
                                rb.isKinematic = true;
                                rb.gameObject.transform.parent = blade.transform;
                                heldObject = rb.gameObject;
                            }
                        }
                    }
                }
                else // if blade is out
                {
                    // scroll wheel reels in blade
                    if (Input.mouseScrollDelta.y < 0.0f)
                    {
                        bladeExtended = false;
                        blade.transform.position += blade.transform.forward * -bladeDistance;

                        if (heldEnemy != null) // Heal when enemies are pulled in
                            player.Heal(10.0f);
                    }
                }

            }
            */
        }
    }

    // Finds component in target game object or parents
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
        if (grapple.pullingObject || grapple.pullingToObject)
        {
            m_Rigidbody.velocity = new Vector3(0, 0, 0);
            return;
        }

        m_Rigidbody.velocity = new Vector3(move.x, m_Rigidbody.velocity.y, move.z);
        //print(m_Rigidbody.velocity);
    }

    private void Attack1()
    {
        timeOfLastAttack = Time.time;
        timeOfFirstAttack = timeOfLastAttack;
        firstHitActivated = true;
        sawAnims.SetTrigger("attack1");
        saw.DealDamage();
    }
    private void Attack2()
    {
        timeOfLastAttack = Time.time;
        secondHitActivated = true;
        sawAnims.SetTrigger("attack2");
        float delay = (0.5f + timeOfFirstAttack) - Time.time;
        if (delay < 0.0f) delay = 0.01f;
        float[] mad = { 1.1f, delay };
        StartCoroutine("DealDamage", mad);
    }
    private void Attack3()
    {
        timeOfLastAttack = Time.time;
        thirdHitActivated = true;
        sawAnims.SetTrigger("attack3");
        float delay = (1.1f + timeOfFirstAttack) - Time.time;
        if (delay < 0.0f) delay = 0.01f;
        float[] mad = { 1.5f, delay };
        StartCoroutine("DealDamage", mad);
    }
    private IEnumerator DealDamage(float[] multiplierAndDelay)
    {
        yield return new WaitForSeconds(multiplierAndDelay[1]);
        saw.DealDamage(multiplierAndDelay[0]);
    }

    public void Die()
    {
        m_Rigidbody.constraints = RigidbodyConstraints.None;
        Camera.main.transform.parent = null;
        this.enabled = false;
    }

    public GameObject GetWeaponHinge()
    {
        return weaponHinge;
    }
}
