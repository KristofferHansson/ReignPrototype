using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns enemies
public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool playOnStart = false;
    private int ind = 0;
    private float interval = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart)
            InvokeRepeating("SpawnEnemy", 4.0f, interval);
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

    public void SetEnemyType(GameObject enemyPrefab)
    {
        this.enemyPrefab = enemyPrefab;
    }

    public void SetInterval(float interval)
    {
        this.interval = interval;
        CancelInvoke();
        InvokeRepeating("SpawnEnemy", 0.5f, interval);
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length < 1) return;

        Instantiate(enemyPrefab, spawnPoints[ind++]);

        if (ind > spawnPoints.Length - 1)
            ind = 0;
    }
}
