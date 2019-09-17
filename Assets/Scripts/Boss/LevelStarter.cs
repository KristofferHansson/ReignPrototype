using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    GameObject boss;

    GameObject player;
    [SerializeField]
    float speed;
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
            
            cameraShake.enabled = true;
            print("In here");
            boss.transform.position += new Vector3(0, 2, 0) * speed * Time.deltaTime;
            if (boss.transform.position.y > -10)
            {
                bossEntered = true;
                
            }
                
            
        }
    }
}
