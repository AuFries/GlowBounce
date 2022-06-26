using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public TMP_Text scoreText;
    public TMP_Text creditText;

    public float currentTimeScale;

    public int score; //Each score increases timescale by 0.001
    public int credits;

    private int demonSpawnThreshold = 50;

    private LevelGenerator LG;

    // Start is called before the first frame update
    void Start()
    {
        LG = FindObjectOfType<LevelGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        score = (int) Camera.main.transform.position.y;
        currentTimeScale = Mathf.Clamp(0.6f + ((float)score / 1000), 0.6f, 1.5f);
        scoreText.text = score.ToString();
        creditText.text = "<sprite index=0> " + credits;
        CheckSpawnDemon();
    }

    public void addCredits(int amount)
    {
        credits += amount;
    }

    private void CheckSpawnDemon()
    {
        if (score >= demonSpawnThreshold)
        {
            LG.SpawnDemonEnemy();
            demonSpawnThreshold += 50;
        }
    }
}
