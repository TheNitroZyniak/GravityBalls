using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {


    [SerializeField] string androidAdID = "Interstitial_Android";
    [SerializeField] string iOSAdID = "Interstitial_iOS";

    private string adID;

    private void Awake() {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdID : androidAdID;
        LoadAd();
    }

    public void LoadAd() {
        //Debug.Log("Loading Ad: " + adID);
        //Debug.Log("" + adID);
        Advertisement.Load(adID, this);
    }

    public void ShowAd() {
        //Debug.Log("Showing Ad: " + adID);
        //Debug.Log("" + adID);
        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        //Debug.Log("Ads loaded");
        //Debug.Log("");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {
        //Debug.Log("Ads started");
        //Debug.Log("");
    }

    public void OnUnityAdsShowClick(string placementId) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        LoadAd();
    }

}
