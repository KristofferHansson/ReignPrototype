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
    [SerializeField] private Sprite attack;
    [SerializeField] private Sprite navigate;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text grappleIndicator; // text by character
    public Text fog;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject startPanel;

    // GameObject definition, not instance
    [SerializeField] private GameObject grapplePointIndicatorPrefab; // create indicator when something is grappleable and in range
    private GameObject grapplePointIndicator; // indicator atop grappleable object
    private RectTransform grapplePointIndicatorTransform;


    private bool gameOver = false;

    public static Vector2 Offset { get; private set; } // used by Enemy to get hp bar position

    void Start()
    {
        if(fog != null)
            fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 0); 
        RectTransform rt = c.GetComponent<RectTransform>();
        Offset = new Vector2(rt.sizeDelta.x / 2f, rt.sizeDelta.y / 2f);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        //startPanel.SetActive(true); // tutorial/intro message

        // Initialize grapple indicator and text
        grapplePointIndicator = Instantiate(grapplePointIndicatorPrefab);
        grapplePointIndicator.transform.SetParent(GameObject.Find("UIPanel").transform, false);
        grapplePointIndicatorTransform = grapplePointIndicator.GetComponent<RectTransform>();
        grapplePointIndicator.gameObject.SetActive(false);
        grappleIndicator.gameObject.SetActive(false);
    }
    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
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
            grapplePointIndicator.SetActive(false);
        }

        else
        {
            grappleIndicator.text = distance.ToString("0.0") + " m";
            grappleIndicator.color = Color.green;
            grappleIndicator.gameObject.SetActive(true);
            grapplePointIndicator.SetActive(true);
        }
    }

    public void UpdateGrappleIndicatorLocation(Vector3 grappleableObjectPosition)
    {
        Vector3 temp = Camera.main.WorldToViewportPoint(grappleableObjectPosition /*+ new Vector3(0.0f, 3.0f, 0.0f)*/);
        grapplePointIndicatorTransform.anchoredPosition = new Vector2(temp.x, temp.y) * Offset * 2f - Offset;
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
