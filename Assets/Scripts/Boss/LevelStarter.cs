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
    CameraShake cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerMaster");
        boss = GameObject.Find("Boss");
        cameraShake = GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        WalkForward();
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

        }
    }
}
