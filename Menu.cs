using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider musicSlider;
    public Slider SFXSlider;

    public TMP_Text[] statsText;

    public TMP_Text creditsText;

    public GameObject upgradesPanel;
    public GameObject customizePanel;
    public GameObject statsPanel;

    public Image settingsButtonImage;
    public GameObject settingsPanel;
    private bool settingsOpen = false;
    private Animator settingsAnim;

    private Sprite upArrow;
    private Sprite gear;
    private Sprite leftArrow;


    private AudioManager AM;

    private GameObject activePanel;

    private bool slidePanelOpen = false;

    public GameObject adsButton;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        settingsAnim = settingsPanel.GetComponent<Animator>();
        upArrow = Resources.Load<Sprite>("Sprites/UpArrow");
        leftArrow = Resources.Load<Sprite>("Sprites/LeftArrow");
        gear = Resources.Load<Sprite>("Sprites/Gear");
        AM = FindObjectOfType<AudioManager>();
        UpdateStatsText();
        UpdateSliders();
    }

    // Update is called once per frame
    void Update()
    {
        creditsText.text = "<sprite index=0> " + PlayerPrefs.GetInt("Credits").ToString();
        if (PlayerPrefs.GetInt("RemovedAds") == 1)
        {
            UpdateUIForRemovedAds();
        }
    }

    private void UpdateUIForRemovedAds()
    {
        Destroy(adsButton);
        creditsText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, creditsText.GetComponent<RectTransform>().anchoredPosition.y);
    }


    public void MoveSettingsPanel()
    {
        if (!slidePanelOpen)
        {
            if (activePanel != null)
            {
                activePanel.SetActive(false);
            }
            if (!settingsOpen) //open
            {
                settingsButtonImage.sprite = upArrow;
                settingsAnim.SetBool("SettingsOpen", true);
            }
            else //close
            {
                settingsButtonImage.sprite = gear;
                settingsAnim.SetBool("SettingsOpen", false);
            }
            settingsOpen = !settingsOpen;
        } else if (activePanel.name ==  "Upgrades Panel")
        {
            MoveUpgradesPanel();
        } else if (activePanel.name == "Customize Panel")
        {
            MoveCustomizePanel();
        } else if (activePanel.name == "Stats Panel")
        {
            MoveStatsPanel();
        }
        
    }


    public void MoveUpgradesPanel()
    {
        if (activePanel != null && activePanel != upgradesPanel)
        {
            activePanel.SetActive(false);
        }
        
        Animator anim = upgradesPanel.GetComponent<Animator>();

        if (anim.GetBool("PanelOpen"))
        {
            anim.SetBool("PanelOpen", false);
            settingsButtonImage.sprite = gear;
            slidePanelOpen = false;
        } else
        {
            upgradesPanel.SetActive(true);
            anim.SetBool("PanelOpen", true);
            settingsButtonImage.sprite = leftArrow;
            slidePanelOpen = true;
        }
        activePanel = upgradesPanel;
    }

    public void MoveCustomizePanel()
    {
        if (activePanel != null && activePanel != customizePanel)
        {
            activePanel.SetActive(false);
        }
        
        Animator anim = customizePanel.GetComponent<Animator>();

        if (anim.GetBool("PanelOpen"))
        {
            anim.SetBool("PanelOpen", false);
            settingsButtonImage.sprite = gear;
            slidePanelOpen = false;
        }
        else
        {
            customizePanel.SetActive(true);
            anim.SetBool("PanelOpen", true);
            settingsButtonImage.sprite = leftArrow;
            slidePanelOpen = true;
        }

        activePanel = customizePanel;
    }

    public void MoveStatsPanel()
    {
        if (activePanel != null && activePanel != statsPanel)
        {
            activePanel.SetActive(false);
        }
        
        Animator anim = statsPanel.GetComponent<Animator>();

        if (anim.GetBool("PanelOpen"))
        {
            anim.SetBool("PanelOpen", false);
            settingsButtonImage.sprite = gear;
            slidePanelOpen = false;
        }
        else
        {
            statsPanel.SetActive(true);
            anim.SetBool("PanelOpen", true);
            settingsButtonImage.sprite = leftArrow;
            slidePanelOpen = true;
        }
        activePanel = statsPanel;
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
        AM.StopSound("Menu");
        AM.Play("Main Theme");
    }

    public void UpdateStatsText()
    {
        statsPanel.SetActive(true);

        statsText[0].text = "Total Score: " + PlayerPrefs.GetInt("TotalScore");

        statsText[1].text = "High Score: " + PlayerPrefs.GetInt("Highscore");

        statsText[2].text = "Total Credits Earned: " + PlayerPrefs.GetInt("TotalCredits");

        statsText[3].text = "Total Deaths: " + PlayerPrefs.GetInt("NumDeaths");

        statsPanel.SetActive(false);
    }

    private void UpdateSliders()
    {
        if (!PlayerPrefs.HasKey("MusicSlider"))
        {
            PlayerPrefs.SetFloat("MusicSlider", 1f);
        } else
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicSlider");
        }

        if (!PlayerPrefs.HasKey("SFXSlider"))
        {
            PlayerPrefs.SetFloat("SFXSlider", 1f);
        } else
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXSlider");
        }
    }
    
    public void setMusicVol(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicSlider", sliderValue);
    }

    public void setSFXVol(float sliderValue)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXSlider", sliderValue);
        AM.Play("Bounce");
    }

    public void RateGame()
    {
        Debug.Log(Application.productName);
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
    }


}
