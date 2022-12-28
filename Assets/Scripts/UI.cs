using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour{
    public static int score;
    [SerializeField] TextMeshProUGUI tmPro;

    private void Start() {
        score = 0;    
    }

    private void Update() {
        tmPro.text = score.ToString();
    }

    public void OnFastClick() {
        Time.timeScale = 8;
    }

    public void OnMenu() {
        GameManager.balls.Clear();
        GameManager.figures.Clear();
        Time.timeScale = 1;

        if(PlayerPrefs.GetInt("BestScore") < score)
            PlayerPrefs.SetInt("BestScore", score);

        SceneManager.LoadScene("GameScene");
    }

    public void OnPauseAndContinue(bool pause) {
        if(pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
