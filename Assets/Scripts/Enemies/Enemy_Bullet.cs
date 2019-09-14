using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public Vector3 target;
    public float Bullet_Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = Vector2.MoveTowards(transform.position, target, Bullet_Speed* Time.deltaTime);
        GetComponent<Rigidbody>().velocity = target.normalized * Bullet_Speed;
        //GetComponent<Rigidbody2D>().MovePosition(new Vector2((transform.position.x + target.x * Bullet_Speed * Time.deltaTime),
        //                                                    (transform.position.y + target.y * Bullet_Speed * Time.deltaTime)));
    }
}
