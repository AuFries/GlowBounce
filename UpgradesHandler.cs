using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpgradesHandler : MonoBehaviour
{
    public Upgrade[] upgrades;

    public string[] infoText;

    IDictionary<int, string> idName;

    private AudioManager AM;

    public GameObject infoTextPanel;

    // Start is called before the first frame update
    void Start()
    {
        AM = FindObjectOfType<AudioManager>();

        if (!PlayerPrefs.HasKey("PlayerHealth"))
        {
            PlayerPrefs.SetInt("PlayerHealth", 100);
            PlayerPrefs.SetInt("PlayerHealthN", 0);
        }

        if (!PlayerPrefs.HasKey("PlayerDefense"))
        {
            PlayerPrefs.SetInt("PlayerDefense", 0);
            PlayerPrefs.SetInt("PlayerDefenseN", 0);
        }

        if (!PlayerPrefs.HasKey("PlayerLuck"))
        {
            PlayerPrefs.SetInt("PlayerLuck", 0);
            PlayerPrefs.SetInt("PlayerLuckN", 0);
        }


        idName = new Dictionary<int, string>() {
            {0, "PlayerHealth"},
            {1, "PlayerDefense"},
            {2, "PlayerLuck"}

        };

        UpdateUpgradesArray();
        UpdateUpgradeVisuals();
    }


    //Price = base cost * 1.15^number purchased
    private void UpdateUpgradesArray()
    {
        foreach (Upgrade u in upgrades)
        {
            string prefName = idName[u.id];
            u.current = PlayerPrefs.GetInt(prefName);
            u.price = (int) (u.basePrice * Mathf.Pow(1.15f,PlayerPrefs.GetInt(prefName + "N")));
        }
    }


    private void UpdateUpgradeVisuals()
    {
        foreach (Upgrade u in upgrades)
        {
            u.panel.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "-<sprite index=0> " + u.price;
            u.panel.transform.GetChild(5).GetComponent<TMP_Text>().text = u.current + u.afterCurrent;
        }
    }

    public void AttemptPurchase(int id)
    {
        Upgrade attempted = Array.Find(upgrades, u => u.id == id);
        string prefName = idName[attempted.id];

        if (PlayerPrefs.GetInt("Credits") >= attempted.price)
        {
            AM.Play("Bought");
            if (prefName == "PlayerHealth")
            {
                PlayerPrefs.SetInt(prefName, PlayerPrefs.GetInt(prefName) + 5);
            }
            else
            {
                PlayerPrefs.SetInt(prefName, PlayerPrefs.GetInt(prefName) + 1);
            }

            PlayerPrefs.SetInt("Credits", PlayerPrefs.GetInt("Credits") - attempted.price);
            PlayerPrefs.SetInt(prefName + "N", PlayerPrefs.GetInt(prefName + "N") + 1);
        } else
        {
            AM.Play("Error");
        }
        
        UpdateUpgradesArray();
        UpdateUpgradeVisuals();
    }

    public void ShowInfoText(int index)
    {
        infoTextPanel.SetActive(true);
        infoTextPanel.GetComponentInChildren<TMP_Text>().text = infoText[index];
    }

    public void CloseInfoText()
    {
        infoTextPanel.SetActive(false);
    }
}
