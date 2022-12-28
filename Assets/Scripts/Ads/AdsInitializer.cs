using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener {

    [SerializeField] string androidGameID = "5003759";
    [SerializeField] string iOSGameID = "5003758";
    [SerializeField] bool testMode = true;
    private string gameID;

    private void Awake() {
        InitializeAds();
    }

    public void InitializeAds() {
        gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameID : androidGameID;
        Advertisement.Initialize(gameID, testMode, this);
    }

    public void OnInitializationComplete() {
        //Debug.Log("Unity Ads init complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        //Debug.Log($"Unity Ads init failed: {error.ToString()} - {message}");
    }
}
