using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {

    [SerializeField] string androidAdID = "Rewarded_Android";
    [SerializeField] string iOSAdID = "Rewarded_iOS";
    //[SerializeField] Button buttonShowID;

    private string adID;

    private void Awake() {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdID : androidAdID;
        //buttonShowID.interactable = false;
    }

    private void Start() {
        //LoadAd();
    }

    public void LoadAd() {
        Debug.Log("Loading Rew Ad: " + adID);
        Advertisement.Load(adID, this);
    }

    public void ShowAd() {
        print(gameObject.name);
        Debug.Log("Showing Rew Ad: " + adID);
        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsAdLoaded(string adUnityID) {
        Debug.Log("Rew Ads loaded: " + adUnityID);

        if (adUnityID.Equals(adID)) {
            //buttonShowID.onClick.AddListener(ShowAd);
            //buttonShowID.interactable = true;
        }

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId) {
        Debug.Log("Ads started");
    }

    public void OnUnityAdsShowClick(string placementId) {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string adUnityID, UnityAdsShowCompletionState showCompletionState) {
        if (adUnityID.Equals(adID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            Debug.Log("Unity Ads Rewarded Ad Completed");

        Time.timeScale = 1;
        //LoadAd();
    }

    private void OnDestroy() {
        //buttonShowID.onClick.RemoveAllListeners();
    }
}
