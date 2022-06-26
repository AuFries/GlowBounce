using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIhandler : MonoBehaviour
{

    public bool gamePaused = false;

    private ScoreManager SM;

    public GameObject pauseMenuUI;
    public GameObject pauseButton;

    private AudioManager AM;

    public GameObject gameOverUI;


    // Start is called before the first frame update
    void Start()
    {
        SM = FindObjectOfType<ScoreManager>();
        AM = FindObjectOfType<AudioManager>();
    }


    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void UnPauseGame()
    {
        gamePaused = false;
        Time.timeScale = SM.currentTimeScale;
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        AM.StopSound("Main Theme");
        AM.Play("Menu");
    }


    public void ShowGameOverPanel()
    {
        pauseButton.SetActive(false);
        gameOverUI.SetActive(true);
        gameOverUI.transform.Find("Score Text").GetComponent<TMP_Text>().text = "Score: " + SM.score;
        if (SM.score > PlayerPrefs.GetInt("Highscore"))
        {
            gameOverUI.transform.Find("Best Text").GetComponent<TMP_Text>().text = "New Best!";
        } else
        {
            gameOverUI.transform.Find("Best Text").GetComponent<TMP_Text>().text = "Best: " + PlayerPrefs.GetInt("Highscore");
        }
        
        gameOverUI.transform.Find("Credits Text").GetComponent<TMP_Text>().text = "+<sprite index=0> " + SM.credits;
    }
}
