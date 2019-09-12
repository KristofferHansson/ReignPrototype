using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Default level script with no progression
public class L00_Default : MonoBehaviour, ILevelScript
{
    [SerializeField] UIMiddleman ui;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (ui is null)
            ui = GameObject.Find("UIMaster").GetComponent<UIMiddleman>();
    }

    public void IncrementScore()
    {
        ui.SetScore(++score);
    }
}
