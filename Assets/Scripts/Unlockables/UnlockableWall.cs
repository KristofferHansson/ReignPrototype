using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableWall : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> keys;
    bool isUnlocked = false;
    public bool needEnemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForKey();
        CheckForKeysAndEnemies();
        CheckForUnlocked();
    }
    void CheckForUnlocked()
    {
        if (isUnlocked)
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
    void CheckForKeysAndEnemies()
    {
        if (!needEnemies)
            return;
        if (keys.Count < 1 && enemies.Count < 1)
        {
            isUnlocked = true;
        }
        else if (keys.Count >= 1 || enemies.Count >= 1)
        {
            isUnlocked = false;
        }
    }

    void CheckForKey()
    {
        if (needEnemies)
            return;
        if (keys.Count < 1)
        {

            isUnlocked = true;
        }
        else if(keys.Count > 0)
        {
            isUnlocked = false;
        }
    }
}
