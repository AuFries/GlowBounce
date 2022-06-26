using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public GameObject tutorialPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("TutorialComplete"))
        {
            ShowTutorial();
        }
    }

    private void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        PlayerPrefs.SetString("TutorialComplete", "Yes");
        Destroy(tutorialPanel, 3.5f);
    }
}
