using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour{
    [SerializeField] Elevator elevator;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform[] points5, points6, points6_1, points5_1;
    [SerializeField] Figure[] figurePrefs;

    [SerializeField] DoubleSize doubleSizePref;
    [SerializeField] PlusBall plusBallPref;

    [SerializeField] Ball ballPrefab;
    public static List<Ball> balls = new List<Ball>();
    public static List<Figure> figures = new List<Figure>();

    [SerializeField] GameObject rewardedBanner, fastClick;

    [SerializeField] AudioSource[] audios;


    [SerializeField] InputManager inputManager;

    [SerializeField] TextMeshProUGUI text;

    public static bool audioPlay;

    public static int ballsCount;
    public static bool isAdsShowed;

    int lineType = 0;
    int bestScore;

    public static float screenRatio;
    [SerializeField] Transform borders;
    [SerializeField] Transform[] points;

    private void Awake() {
        float defaultRatio = 1080f/1920f;
        float newRatio = (float)Screen.width / (float)Screen.height;
        if (Screen.width < Screen.height)
            screenRatio = (float)newRatio / (float)defaultRatio;
        else
            screenRatio = 1;
    }

    private void Start() {
        borders.localScale = new Vector3(borders.localScale.x * screenRatio, borders.localScale.y, borders.localScale.z);
        for(int i = 0; i < points.Length; i++) {
            points[i].position = new Vector3(points[i].position.x * screenRatio, points[i].position.y, points[i].position.z);
        }


        isAdsShowed = false;
        ballsCount = 0;
        for (int i = 0; i < 8; i++) {
            Ball ball = Instantiate(ballPrefab, new Vector3(Random.Range(-2f, 2f), Random.Range(5f, 10f), 0), Quaternion.identity);
        }
        StartCoroutine(CreateLine(points5_1, 3));
        StartCoroutine(CreateLine(points6_1, 2));
        StartCoroutine(CreateLine(points5, 1));

        bestScore = PlayerPrefs.GetInt("BestScore");
        text.text = "Рекорд: " + bestScore.ToString();

    }

    float fastTime;

    private void Update() {
        if (balls.Count == ballsCount && balls.Count != 0) {
            lineType++;
            Time.timeScale = 1;
            bool doIt = true;

            fastClick.SetActive(false);

            foreach(Ball ball in balls) {
                ball.isFlying = false;
            }

            foreach (Figure fig in figures) {
                fig.positionY++;
                if (fig.positionY == 9 && fig.gameObject.activeInHierarchy) doIt = false;
            }
            if (doIt) {
                StartCoroutine(elevator.MoveUp());
                if (lineType % 2 == 0) StartCoroutine(CreateLine(points5, 1));
                else StartCoroutine(CreateLine(points6, 1));
            } 
            else {

                if (!isAdsShowed)
                    rewardedBanner.SetActive(true);

                else {
                    balls.Clear();
                    figures.Clear();

                    if (PlayerPrefs.GetInt("BestScore") < UI.score)
                        PlayerPrefs.SetInt("BestScore", UI.score);
                    SceneManager.LoadScene("GameScene");
                }

            }
            

            fastTime = 0;
            ballsCount = 0;
            InputManager.deactivate = false;
        }

        if (InputManager.deactivate) {
            if(fastTime >= 0)
                fastTime += Time.deltaTime;
            if(fastTime > 5 && !fastClick.activeInHierarchy) {
                fastClick.SetActive(true);
                fastTime = -1;
            }
        }
      

        if (audioPlay)
            AudioPlay();

    }


    void AudioPlay() {
        audioPlay = false;
        for (int i = 0; i < audios.Length; i++) {
            if (!audios[i].isPlaying) {
                audios[i].Play();
                break;
            }
        }
    }

    IEnumerator CreateLine(Transform[] points, int position) {
        yield return new WaitForSeconds(1);
        int amount;
        if (balls.Count < 50)
            amount = Random.Range(points.Length - 3, points.Length);
        else
            amount = Random.Range(points.Length - 2, points.Length);

        int plusChance = Random.Range(0, 4);
        int doubleChance = Random.Range(0, 4);

        List<Transform> positions = new List<Transform>();
        for(int i = 0; i < points.Length; i++) {
            positions.Add(points[i]);
        }

        for (int i = 0; i < amount; i++) {
            int r = Random.Range(0, positions.Count);


            Figure fig = Instantiate(figurePrefs[Random.Range(0, figurePrefs.Length)], positions[r].position, Quaternion.Euler(0, 0, Random.Range(-30, 30)));
            fig.positionY = position;
            elevator.SetBlocks(fig.transform);
            figures.Add(fig);
            positions.RemoveAt(r);
        }


        if (balls.Count < 50) {

            if (plusChance == 0) {
                if (positions.Count > 0) {
                    int r = Random.Range(0, positions.Count);
                    PlusBall pb = Instantiate(plusBallPref, positions[r].position, Quaternion.identity);
                    pb.positionY = position;
                    elevator.SetBlocks(pb.transform);
                    positions.RemoveAt(r);
                }
            }

            if (doubleChance == 0) {
                if (positions.Count > 0) {
                    int r = Random.Range(0, positions.Count);
                    DoubleSize db = Instantiate(doubleSizePref, positions[r].position, Quaternion.identity);
                    db.positionY = position;
                    elevator.SetBlocks(db.transform);
                    positions.RemoveAt(r);
                }
            }
        }
        inputManager.SetBallToPoint();

    }
}
