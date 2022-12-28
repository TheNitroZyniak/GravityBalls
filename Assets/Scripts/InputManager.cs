using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour{
    [SerializeField] Ball ballPrefab;
    [SerializeField] Transform startPoint, line, inputPoint;
    [SerializeField] AudioSource shootAudio;

    Vector2 startPos, endPos;
    public static bool deactivate;
    float scl;
    Vector3 startScale, endScale;

    [SerializeField] GameObject menuScene, gameScene;
    [SerializeField] GameObject pauseObject, continueVideo;

    private void Start() {
        startScale = new Vector3(0.5f, 0.5f, 1);
        endScale = new Vector3(0.7f, 0.7f, 1);
        deactivate = false;    
    }

    void Update(){

        if (EventSystem.current.currentSelectedGameObject != null) {
            if (EventSystem.current.currentSelectedGameObject.layer == 5) return;
        }

        if (pauseObject.activeInHierarchy) return;
        if (continueVideo.activeInHierarchy) return;



        if (!deactivate) {
            float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            if (y < 2) {
                if (Input.GetMouseButtonDown(0)) {
                    menuScene.SetActive(false);
                    gameScene.SetActive(true);
                    line.gameObject.SetActive(true);
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    
                }
                if (Input.GetMouseButton(0)) {
                    endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    inputPoint.position = endPos;
                    Vector3 targetDir = inputPoint.position - startPoint.position;
                    float angle = Vector3.Angle(targetDir, startPoint.right);
                    line.rotation = Quaternion.Euler(0, 0, -angle);

                    float dist = Vector2.Distance(endPos, startPoint.position);
                    if (dist < 3) scl = 0;
                    else if (dist >= 3 && dist < 6) {
                        scl = (float)(dist - 3) / 3f;
                    } 
                    else scl = 1;

                    line.localScale = Vector3.Lerp(startScale, endScale, scl);
                }
                if (Input.GetMouseButtonUp(0)) {
                    
                    line.gameObject.SetActive(false);
                    endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dir = new Vector2(startPoint.position.x, startPoint.position.y) - endPos;
                    deactivate = true;
                    StartCoroutine(Shoot(-dir));
                }
            }
        }
    }
    IEnumerator Shoot(Vector2 dir) {

        for (int i = 0; i < GameManager.balls.Count; i++) {
            int ballID = SetBallToPoint();

            if (shootAudio.isPlaying)
                shootAudio.Stop();
            shootAudio.Play();
            GameManager.balls[ballID].Push(dir);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public int SetBallToPoint() {
        float dist = 1000;
        int ballID = 0;
        for (int j = 0; j < GameManager.balls.Count; j++) {
            if (!GameManager.balls[j].isFlying) {
                float dist2 = Vector2.Distance(GameManager.balls[j].transform.position, new Vector3(0, 3.25f, 0));
                if (dist2 < dist) {
                    dist = dist2;
                    ballID = j;
                }
            }
        }
        GameManager.balls[ballID].transform.position = new Vector3(0, 3.25f, 0);
        GameManager.balls[ballID].SetParams();
        return ballID;
    }
}