using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] public float DamagePerSecond { get; private set; } = 200.0f;
    [SerializeField] private UIMiddleman ui;
    [SerializeField] public Light sawLight;
    PlayerController pc;
    private float health;
    public float HeatLevel { get; private set; } = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();

        pc = GetComponent<PlayerController>();
        health = maxHealth;
        StartCoroutine("CoolDownHeatRepeating", 0.1f);
    }

    public void TakeDamage(float amt)
    {
        health -= amt;
        ui.SetPlayerHP(health / maxHealth);
        //print("Player health is " + health + ".");
        if (health <= 0)
        {
            // die
            pc.Die();
            ui.GameOver();
            //print("Player is dead.");
        }
    }

    public void Heal(float amt)
    {
        health += amt;
        ui.SetPlayerHP(health / maxHealth);
        if (health > maxHealth)
            health = maxHealth;
    }

    public void IncreaseHeat()
    {
        HeatLevel += 0.1f;
        if (HeatLevel > 3.0f) // heat from 0 to 3
            HeatLevel = 3.0f;
        sawLight.intensity = 3.0f + HeatLevel * 4.0f; // intensity from 3 to 15
        ui.SetPlayerHeat(HeatLevel / 3.0f);
    }

    private void CoolDownHeat()
    {
        HeatLevel -= 0.01f;
        if (HeatLevel < 0.0f)
            HeatLevel = 0.0f;
        ui.SetPlayerHeat(HeatLevel / 3.0f);
    }

    // Takes in damage amount and interval as floats
    private IEnumerator CoolDownHeatRepeating(float interval)
    {
        while (true)
        {
            CoolDownHeat();
            yield return new WaitForSeconds(interval);
        }
    }
}
