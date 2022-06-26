using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "4243406";
    private string mySurfacingId = "Rewarded_iOS";
    private string interstitialID = "Interstitial_iOS";
#elif UNITY_ANDROID
    private string gameId = "4243407";
    private string mySurfacingId = "Rewarded_Android";
    private string interstitialID = "Interstitial_Android";
#endif

    bool testMode = false;

    private bool showedAdLastRound = true;

    public static AdsManager instance;

    public GameObject adsRemovedPanel;

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

        if (!PlayerPrefs.HasKey("RemovedAds"))
        {
            PlayerPrefs.SetInt("RemovedAds", 0);
        }
        DontDestroyOnLoad(gameObject);
        // Initialize the Ads service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId))
        {
            Advertisement.Show(mySurfacingId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            if (surfacingId == "Rewarded_Android" || surfacingId == "Rewarded_iOS")
            {
                // Reward the user for watching the ad to completion
                Debug.Log("REWARD GIVEN");
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    public void TryShowAd() //Call on player death
    {
        if (PlayerPrefs.GetInt("RemovedAds") != 1)
        {
            if (!showedAdLastRound) //Show ad
            {
                if (Advertisement.IsReady())
                {
                    Advertisement.Show(interstitialID);
                    showedAdLastRound = true;
                }
            }
            else
            {
                showedAdLastRound = false;
            }
        }
        
    }

    public void OnRemovedAdsPurchaseComplete()
    {
        PlayerPrefs.SetInt("RemovedAds", 1);
        adsRemovedPanel.SetActive(true);
        Destroy(adsRemovedPanel, 3f);
    }
}

