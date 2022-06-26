using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("TotalScore"))
        {
            PlayerPrefs.SetInt("TotalScore", 0);
        }

        if (!PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }

        if (!PlayerPrefs.HasKey("TotalCredits")) {
            PlayerPrefs.SetInt("TotalCredits", 0);
        }

        if (!PlayerPrefs.HasKey("Credits"))
        {
            PlayerPrefs.SetInt("Credits", 0);
        }

        if (!PlayerPrefs.HasKey("NumDeaths")) {
            PlayerPrefs.SetInt("NumDeaths", 0);
        }
    }


    public static void AddDeath()
    {
        PlayerPrefs.SetInt("NumDeaths", PlayerPrefs.GetInt("NumDeaths") + 1);
    }

    public static void AddCredits(int amount) //Adds credits to prefs
    {
        PlayerPrefs.SetInt("TotalCredits", PlayerPrefs.GetInt("TotalCredits") + amount);
        PlayerPrefs.SetInt("Credits",PlayerPrefs.GetInt("Credits") + amount);
    }

    public static void SpendCredits(int amount)
    {
        //Fix later
    }

    public static void Score(int score) //Adds score to total score and checks if a new highscore was acheived
    {
        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + score);

        if (PlayerPrefs.GetInt("Highscore") < score)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

}
