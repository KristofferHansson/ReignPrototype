using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the progression of the level
public class L01_IntroLevel : MonoBehaviour, ILevelScript
{
    [SerializeField] Spawner[] spawners;
    [SerializeField] UIMiddleman ui;
    [SerializeField] AudioSource mainAudio;
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] AudioClip gong;

    private int score = 0;
    private int scoreAtPhase2 = 1000;
    private int scoreAtPhase3 = 1000;

    // Start is called before the first frame update
    void Start()
    {
        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();

        if (mainAudio is null || mainAudio == null)
            mainAudio = GameObject.Find("MainAudio").GetComponent<AudioSource>();

        if (spawners.Length == 0)
        {
            spawners = FindObjectsOfType<Spawner>();
        }
    }

    public void BeginPlay()
    {
        foreach (Spawner s in spawners)
        {
            s.SetInterval(1.5f);
            PlayGongSound();
        }
    }

    public void IncrementScore()
    {
        ui.SetScore(++score);
        
        if (score == 10)
        {
            SetInt(0.5f);
            PlayGongSound();
            scoreAtPhase2 = score;
        }

        else if (score == scoreAtPhase2 + 20)
        {
            print("Congratulations, you win.");
            SetInt(10.0f);
            Invoke("JustKidding", 8.0f);
        }

        else if (score == scoreAtPhase3 + 10)
        {
            print("You win. No, really this time.");
            SetInt(100.0f);
        }
    }

    private void JustKidding()
    {
        print("Just kidding.");
        SetEnemy(enemy2); // Change to next enemy type
        SetInt(0.8f);
        PlayGongSound();
        scoreAtPhase3 = score;
        Invoke("StopSpawningEndLevel", 7.0f);
    }

    // Stop spawning, check if win conditions met
    private void StopSpawningEndLevel()
    {
        foreach (Spawner s in spawners)
        {
            s.StopSpawning();
        }

        Invoke("ContinueSpawningScoreNotReached", 8.0f);
    }

    // Spawn burst of enemies if win condition not met
    private void ContinueSpawningScoreNotReached()
    {
        if (score < scoreAtPhase3 + 10)
        {
            SetInt(0.8f);
            Invoke("StopSpawningEndLevel", 7.0f);
        }
        else
        {
            ui.LevelComplete();
        }
    }

    private void SetInt(float i)
    {
        foreach (Spawner s in spawners)
        {
            s.SetInterval(i);
        }
    }

    private void SetEnemy(GameObject e)
    {
        foreach (Spawner s in spawners)
        {
            s.SetEnemyType(e);
        }
    }

    private void PlayGongSound()
    {
        if (!mainAudio || !gong)
            return;

        mainAudio.clip = gong;
        mainAudio.Play();
    }
}
