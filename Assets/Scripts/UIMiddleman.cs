using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Changes UI values
public class UIMiddleman : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider heatSlider;
    [SerializeField] private Canvas c;
    [SerializeField] private Image grappleMode;
    [SerializeField] private Sprite attack;
    [SerializeField] private Sprite navigate;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text grappleIndicator;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject startPanel;


    private bool gameOver = false;

    public static Vector2 Offset { get; private set; } // used by Enemy to get hp bar position

    void Start()
    {
        RectTransform rt = c.GetComponent<RectTransform>();
        Offset = new Vector2(rt.sizeDelta.x / 2f, rt.sizeDelta.y / 2f);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        startPanel.SetActive(true); // tutorial/intro message
        grappleIndicator.gameObject.SetActive(false);
    }

    public void SetGrappleMode(bool combatMode)
    {
        if (combatMode)
            grappleMode.sprite = attack;
        else
            grappleMode.sprite = navigate;
    }
    public void SetPlayerHP(float fracRemaining)
    {
        if (fracRemaining < 0) fracRemaining = 0;
        hpSlider.value = fracRemaining;
    }

    public void SetPlayerHeat(float frac)
    {
        if (frac < 0) frac = 0;
        heatSlider.value = frac;
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    // Show distance of grappleable object with label near cursor
    public void ShowGrappleIndicator(float distance)
    {
        if (distance < 0.0f)
        {
            grappleIndicator.text = "Grapple unavailable";
            grappleIndicator.color = Color.grey;
            grappleIndicator.gameObject.SetActive(true);
        }

        else
        {
            grappleIndicator.text = distance.ToString("0.0") + " m";
            grappleIndicator.color = Color.green;
            grappleIndicator.gameObject.SetActive(true);
        }
    }

    public void LevelComplete()
    {
        if (!gameOver)
            victoryPanel.SetActive(true);
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void EHRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EHQuit()
    {
        Application.Quit();
    }

    public void EHClose()
    {
        startPanel.SetActive(false);
    }
}
