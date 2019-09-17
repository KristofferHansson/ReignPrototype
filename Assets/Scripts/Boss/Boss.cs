using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private bool dead = false;
    [SerializeField] private UIMiddleman ui;
    private float rotRate = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();
    }

    public void TakeDamage()
    {
        health += -8.34f; // for real gameplay
        //health += -20f; // for demo
        //rotRate += 1.3f;
        rotRate += 1f;
        Attack();
        if (health < 0)
            Die();
    }

    private void Attack()
    {
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        while (true)
        {
            this.transform.Rotate(0,rotRate * Time.deltaTime,0);
            yield return new WaitForEndOfFrame();
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        dead = true;
        StartCoroutine(Descend());
    }

    IEnumerator Descend()
    {
        while (true)
        {
            this.transform.position += new Vector3(0, -3.0f * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
            if (this.transform.position.y < -30.0f)
                break;
        }
        ui.LevelComplete();
    }
}
