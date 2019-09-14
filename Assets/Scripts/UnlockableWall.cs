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
        CheckForEnemies();
        CheckForKey();
        CheckForKeysAndEnemies();
    }
    void CheckForUnlocked()
    {
        if (isUnlocked)
            gameObject.SetActive(false);
    }
    void CheckForEnemies()
    {
        if (enemies.Count < 1 && needEnemies)
        {
            isUnlocked = true;
        }
    }

    void CheckForKey()
    {
        if (keys.Count < 1 && !needEnemies)
        {
            isUnlocked = true;
        }
    }

    void CheckForKeysAndEnemies()
    {
        if(keys.Count <1 && enemies.Count <1 && needEnemies)
        {
            isUnlocked = true;
        }
    }
}
