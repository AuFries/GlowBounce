using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using UnityEngine.UI;

public class CustomizeHandler : MonoBehaviour
{
    public Skins[] skins;

    //255 255 255 alpha 100

    private Skins selectedSkin;

    private Color selectedColor;
    private Color deselectedColor;
    private Color lockedColor;

    private static Color playerColor;

    private AudioManager AM;

    // Start is called before the first frame update
    void Start()
    {
        selectedColor = new Color(255,255,255,100);
        deselectedColor = new Color(255, 255, 255, 0);
        lockedColor = new Color(0,0,0,100);

        AM = FindObjectOfType<AudioManager>();

        if (!PlayerPrefs.HasKey("SelectedSkin"))
        {
            selectedSkin = skins[0];
            selectedSkin.bought = 1;
            PlayerPrefs.SetString("SelectedSkin", "Yellow");
            PlayerPrefs.SetInt(selectedSkin.name + " bought", 1);
        }
        else
        {
            selectedSkin = Array.Find(skins, skin => skin.name == PlayerPrefs.GetString("SelectedSkin"));
        }

        foreach (Skins s in skins)
        {
            if (!PlayerPrefs.HasKey(s.name + " bought"))
            {
                PlayerPrefs.SetInt(s.name + " bought", 0);
            } else
            {
                s.bought = PlayerPrefs.GetInt(s.name + " bought");
            }
            
        }

        UpdateSelectedVisuals();
        ChangeMenuPlayerColors();
    }


    public void selectSkin(string name)
    {
        Skins attemptedSelect = Array.Find(skins, skin => skin.name == name); ;

        if (PlayerPrefs.GetInt(name + " bought") == 1)  {//check bought already
            selectedSkin = attemptedSelect;
            PlayerPrefs.SetString("SelectedSkin", selectedSkin.name);
            AM.Play("Selected");
        } else if (PlayerPrefs.GetInt("Credits") >= attemptedSelect.price) //otherwise purchase and select
        {
            PlayerPrefs.SetInt("Credits", PlayerPrefs.GetInt("Credits") - attemptedSelect.price);
            selectedSkin = attemptedSelect;
            selectedSkin.bought = 1;
            PlayerPrefs.SetInt(name + " bought", 1); 
            PlayerPrefs.SetString("SelectedSkin", selectedSkin.name);
            AM.Play("Bought");
        } else
        {
            AM.Play("Error");
        }

        UpdateSelectedVisuals();
        ChangeMenuPlayerColors();
    }


    private void UpdateSelectedVisuals()
    {

        foreach (Skins s in skins)
        {
            if (s.bought == 1)
            {
                if (s == selectedSkin)
                {
                    s.panel.GetComponent<Image>().color = selectedColor;
                    s.panel.transform.GetChild(2).gameObject.SetActive(true);
                    playerColor = s.color;
                } else
                {
                    s.panel.GetComponent<Image>().color = deselectedColor;
                    s.panel.transform.GetChild(2).gameObject.SetActive(false);
                }
                s.panel.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
            } else
            {
                s.panel.GetComponent<Image>().color = lockedColor;
            }
        }
    }

    public static void ChangePlayerColors(GameObject playerLight, TrailRenderer TR)
    {
        Light2D light2D = playerLight.GetComponent<Light2D>();
        light2D.color = playerColor;
        TR.endColor = playerColor;
    }

    private void ChangeMenuPlayerColors()
    {
        GameObject player = FindObjectOfType<Player>().gameObject;
        TrailRenderer TR = player.GetComponent<TrailRenderer>();
        Light2D light2D = player.transform.GetChild(0).GetComponent<Light2D>();

        light2D.color = playerColor;
        TR.endColor = playerColor;
    }
}
