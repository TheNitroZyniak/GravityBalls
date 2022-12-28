using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardedVideo : MonoBehaviour{
    [SerializeField] Image circle;
    //[SerializeField] InterstitialAds interAds;
    [SerializeField] RewardedAds rewAds;

    private void Start() {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {

        float a = 1;
        while(a > 0) {
            a -= 0.005f;
            circle.fillAmount = a;
            yield return new WaitForFixedUpdate();
        }
        


        //interAds.ShowAd();
        
        GameManager.balls.Clear();
        GameManager.figures.Clear();

        if (PlayerPrefs.GetInt("BestScore") < UI.score)
            PlayerPrefs.SetInt("BestScore", UI.score);

        SceneManager.LoadScene("GameScene");
    }

    public void OnContinue() {
        GameManager.isAdsShowed = true;
        //rewAds.ShowAd();
        for (int i = 0; i < GameManager.figures.Count; i++) {
            if (GameManager.figures[i].positionY == 8 || GameManager.figures[i].positionY == 9) {
                GameManager.figures[i].gameObject.SetActive(false);
            } 
            else
                GameManager.figures[i].positionY--;
        }
    }
}
