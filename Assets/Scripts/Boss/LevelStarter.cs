using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject axe1;
    [SerializeField] private GameObject axe2;
    [SerializeField] private GameObject platform;
    GameObject boss;
    GameObject player;
    bool playerStoppedWalking = false;
    bool playerCanMove = false;
    bool bossEntered = false;
    public bool firstPluck = false;
    
    public CameraShake cameraShake;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerMaster");
        boss = GameObject.Find("Boss");
        cameraShake = GetComponent<CameraShake>();
        print(boss);
    }

    // Update is called once per frame
    void Update()
    {
        WalkForward();
        if(firstPluck)
            EnterBoss();

        if (bossEntered)
            Destroy(this);
    }

    void WalkForward()
    {
        if (playerStoppedWalking)
            return;

        player.transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
        if (player.transform.position.z > -231)
            playerStoppedWalking = true;
    }

    void EnterBoss()
    {
        
        if(playerStoppedWalking && !bossEntered)
        {
            Destroy(platform);
            cameraShake.enabled = true;
            //print("In here");
            boss.transform.position += new Vector3(0, 2, 0) * speed * Time.deltaTime;
            axe1.transform.localPosition += new Vector3(0.12f, 0, 0) * speed * Time.deltaTime;
            axe2.transform.localPosition += new Vector3(-0.12f, 0, 0) * speed * Time.deltaTime;
            if (boss.transform.position.y > -10)
            {
                bossEntered = true;
                
            }
                
            
        }
    }
}
