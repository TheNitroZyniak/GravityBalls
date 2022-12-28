using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Figure : MonoBehaviour{
    SpriteRenderer rend;
    int startLives;
    public TextMeshPro tm;
    [SerializeField] Color[] colors;
    [SerializeField] GameObject expPref;
    [SerializeField] Sprite expSprite;
    public int positionY;

    Vector3 scale;

    

    private void Start() {
        scale = transform.localScale * GameManager.screenRatio;
        transform.localScale = Vector3.zero;

        rend = GetComponent<SpriteRenderer>();
        startLives = Random.Range(1, GameManager.balls.Count * 6);
        ChangeColor();
        SetText(startLives);
        tm.gameObject.transform.rotation = Quaternion.identity;

        StartCoroutine(Expand());
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Ball>().isBig)
            startLives--;

        GameManager.audioPlay = true;
        startLives--;
        if (startLives <= 0) {
            Demilitarize();
        }
        ChangeColor();
        SetText(startLives);
    }

    void SetText(int count) {
        tm.text = count.ToString();
    }

    public void ChangeColor() {
        if (startLives > 0 && startLives < 75) {
            float a = startLives / 75f;
            rend.color = Color.Lerp(colors[4], colors[3], a);
        }
        if (startLives >= 75 && startLives < 150) {
            float a = (startLives - 75) / 75f;
            rend.color = Color.Lerp(colors[3], colors[2], a);
        }
        if (startLives >= 150 && startLives < 225) {
            float a = (startLives - 150) / 75f;
            rend.color = Color.Lerp(colors[2], colors[1], a);
        }
        if (startLives >= 225 && startLives <= 300) {
            float a = (startLives - 225) / 75f;
            rend.color = Color.Lerp(colors[1], colors[0], a);
        }
    }


    public IEnumerator Shake() {
        float a = 0;
        Vector3 startPos = transform.position;
        while(a < 1) {
            transform.position = new Vector3(startPos.x + Random.Range(-0.05f, 0.05f), startPos.y + Random.Range(-0.05f, 0.05f), startPos.z);
            a += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        transform.position = startPos;
    }

    IEnumerator Expand() {
        float a = 0;
        Vector3 startPos = transform.position;
        while (a < 1) {
            transform.localScale = Vector3.Lerp(Vector3.zero, scale, a);
            a += 0.05f;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Demilitarize() {
        GameManager.figures.Remove(this);

        ParticleSystem pt = Instantiate(expPref, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        pt.textureSheetAnimation.SetSprite(0, expSprite);
        Destroy(pt, 2);

        gameObject.SetActive(false);
        Destroy(gameObject, 1);
    }
}
