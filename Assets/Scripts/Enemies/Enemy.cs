using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float maxSpeed = 5.0f; // max speed
    [SerializeField] private Transform player;
    [SerializeField] private Collider hitbox; // disable upon dead
    [SerializeField] private GameObject healthBarPrefab; // create bar in UI upon spawn
    [SerializeField] private UIMiddleman ui;
    [SerializeField] private ILevelScript lvl;
    [SerializeField] private GameObject eyes;
    private float speed;
    private EnemyHPUI healthBar; // to update fill of bar
    private RectTransform hbTransform; // to update position of bar
    private float health;
    private Rigidbody rb;
    private bool dead = false;
    private bool knockedBack = false;
    private bool killZed = false;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        speed = maxSpeed;

        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();
        if (lvl == null || lvl.Equals(default(ILevelScript)))
        {
            Component[] temp = GameObject.Find("LevelMaster").GetComponents(typeof(Component));
            foreach (Component c in temp)
            {
                if (c is ILevelScript)
                {
                    lvl = c as ILevelScript;
                    break;
                }
            }
        }

        // instantiate health bar UI Slider
        healthBar = Instantiate(healthBarPrefab).GetComponent<EnemyHPUI>();
        healthBar.transform.SetParent(GameObject.Find("UIPanel").transform, false);
        hbTransform = healthBar.GetComponent<RectTransform>();
        UpdateHealthBarLocation();

        // Set up RB and player
        rb = GetComponent<Rigidbody>();
        if (!player)
        {
            player = GameObject.Find("PlayerMaster").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -70.0f && !killZed)
            KillZ();

        transform.LookAt(player);
        if (Vector3.Distance(transform.position, player.position) >= 1.2f)
        {
            if (!dead && !knockedBack) // GameObj position
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }

            if (!killZed) // Health bar position
                UpdateHealthBarLocation();
        }
    }

    public void TakeDamage(float amt)
    {
        if (dead) return;

        health -= amt;
        //print("Enemy " + gameObject.name + " health is " + health + "."); // convert to healthbar
        // update healthBar fill value
        float fracRemaining = health / maxHealth;
        if (fracRemaining <= 0) fracRemaining = 0;
        healthBar.SetValue(fracRemaining);

        if (health <= 0)
        {
            // Immediate death actions
            lvl.IncrementScore();
            //player.GetComponent<Player>().IncreaseHeat();
            dead = true;
            rb.constraints = RigidbodyConstraints.None;
            hitbox.enabled = false;
            // Delayed death actions
            Invoke("Die", 0.8f);
        }
    }

    public void KnockBack()
    {
        StartCoroutine(MoveBackOverTime());
    }
    private IEnumerator MoveBackOverTime()
    {
        knockedBack = true;
        for (int i = 0; i < 4; i++) { // move back for 4 frames
            this.transform.position += this.transform.forward * -20.0f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 11; i++) { // stun for 11 frames
            yield return new WaitForEndOfFrame();
        }
        knockedBack = false;
    }

    // amt = points to be dealt every 0.01 seconds
    public void BeginPeriodicDamage(float amt)
    {
        speed = maxSpeed * 0.6f;
        //InvokeRepeating("TakeDamage", 0.0f, 0.001f);
        float[] dmgInt = { amt, 0.01f };
        StartCoroutine("TakeDamageRepeating", dmgInt);
    }

    public void EndPeriodicDamage()
    {
        speed = maxSpeed;
        //CancelInvoke("TakeDamage");
        StopCoroutine("TakeDamageRepeating");
    }

    public void Grab()
    {
        this.enabled = false;
    }

    public void Release()
    {
        if (health > 0.0f)
            this.enabled = true;
    }

    // Takes in damage amount and interval as floats
    private IEnumerator TakeDamageRepeating(float[] dmgInt)
    {
        while (true)
        {
            TakeDamage(dmgInt[0]);
            yield return new WaitForSeconds(dmgInt[1]);
        }
    }

    // To maintain healthBar momentarily after death, invoke this with delay
    private void Die()
    {
        if (!(eyes is null))
            Destroy(eyes);
        Destroy(healthBar.gameObject);
        //this.enabled = false; // if more functionality added after death
        Destroy(this); // remove script component
    }

    // Destroy this GO without affecting score; Use when enemy has dropped out of map bounds
    private void KillZ()
    {
        killZed = true;
        Destroy(healthBar.gameObject);
        Invoke("DestroyGO", 10.0f);
    }
    private void DestroyGO()
    {
        Destroy(this.gameObject);
    }

    private void UpdateHealthBarLocation()
    {
        Vector3 temp = Camera.main.WorldToViewportPoint(this.transform.position + new Vector3(0.0f, 3.0f, 0.0f));
        hbTransform.anchoredPosition = new Vector2(temp.x, temp.y) * UIMiddleman.Offset * 2f - UIMiddleman.Offset;
    }
}
