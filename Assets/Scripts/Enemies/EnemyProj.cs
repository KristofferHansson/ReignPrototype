using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProj : MonoBehaviour
{
    public GameObject player;
    public int Bullet_speed = 10;
    public Grapple grapple;
    public UnlockableWall wallParent;
    public float health = 100;
    private Vector3 Direction_to_Player;
    private float turning_speed = 6f;

    public GameObject Enemy_Bullet;

    private int Attack_timer = 60;

    private GameObject Self_Obj;

    private bool Can_Attack;

    private Vector3 Bullet_dir_player;

    private bool Moving = false;

    public float AttackRate = 3f;

    public float Bullet_Exist_Time;

    public bool In_Range = false;

    public float Attack_Range;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        grapple = GameObject.Find("Grapple").GetComponent<Grapple>();
        player = GameObject.Find("PCollider");
        rb = GetComponent<Rigidbody>();
        Can_Attack = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Turn_Dir_to_Player();
        Range();
        Attack();
        if (rb.velocity.y < -45)
            TakeDamage(100);
        
    }

    void Attack()
    {
        //print(Bullet_dir_player);
        if (Can_Attack)
        {
            if (In_Range)
            {
                
                GameObject Bullet = Instantiate(Enemy_Bullet, transform.position, Quaternion.identity);
                Bullet.transform.parent = gameObject.transform;
                Bullet.GetComponent<Enemy_Bullet>().target = player.transform.position - transform.position;

                Destroy(Bullet, Bullet_Exist_Time);
                //Bullet_dir_player = player.transform.position - transform.position;

                //Bullet.GetComponent<Rigidbody2D>().velocity = Bullet_dir_player.normalized * 6;

                //Bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * Bullet_speed);

                //print(Bullet.GetComponent<Rigidbody2D>().velocity);

                Can_Attack = false;
                StartCoroutine(AttackCooldown());
            }
        }



        else
        {
            return;
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(AttackRate);
        Can_Attack = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Player_Weapon")
        {
            Health -= collision.gameObject.GetComponentInParent<Player>().Damage;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.tag == inserttag)
          TakeDamage();
          */
    }

    void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        if(health < 0)
        {
            Die();
        }
    }

    void Die()
    {
        
        if(grapple.grappledObj == gameObject)
        {
            //Let's player know that they are grappling on to nothing
            grapple.grappledObj = null;
            grapple.pullingObject = false;
            grapple.pullingToObject = false;
            grapple.isGrappled = false;
        }
        if(wallParent != null)
        {
            wallParent.enemies.Remove(gameObject);
        }
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void Range()
    {
        if (player)
        {
            //print(speed);
            if (Vector3.Distance(player.transform.position, transform.position) <= Attack_Range)
            {
                //print(Vector2.Distance(player.transform.position, transform.position));
                In_Range = true;
            }
            else
            {
                //print("Player Leave");
                In_Range = false;
            }
        }

    }
}
