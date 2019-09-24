using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image fill;

    public void SetValue(float frac)
    {
        if (frac <= 0)
            fill.enabled = false;
        else
            hpBar.value = frac;
    }
}
