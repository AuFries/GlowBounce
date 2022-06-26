using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    private ScoreManager SM;
    private AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        SM = FindObjectOfType<ScoreManager>();
        AM = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int numCredits = (PlayerPrefs.GetInt("PlayerLuck") * 5) / 100 + 1;
            int percentForAnother = (PlayerPrefs.GetInt("PlayerLuck") * 5) % 100;

            if (Random.value * 100f <= percentForAnother)
            {
                numCredits += 1;
            }

            SM.addCredits(numCredits);
            AM.Play("Coin");
            Destroy(gameObject);
        }
    }
}
