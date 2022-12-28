using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusBall : MonoBehaviour{
    [SerializeField] Ball newBallPref;
    [SerializeField] GameObject textObject;
    public int positionY;
    

    private void OnTriggerEnter2D(Collider2D collision) {
        
        Rigidbody2D newBall = Instantiate(newBallPref, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        
        for (int j = 0; j < GameManager.balls.Count; j++) 
            Physics2D.IgnoreCollision(newBall.gameObject.GetComponent<Collider2D>(), GameManager.balls[j].GetComponent<Collider2D>());
        
        Rigidbody2D rb2 = collision.gameObject.GetComponent<Rigidbody2D>();
        newBall.velocity = -rb2.velocity;
        newBall.angularVelocity = -rb2.angularVelocity;
        newBall.gravityScale = rb2.gravityScale;

        newBall.GetComponent<Ball>().SetPM2D();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(MoveUP());
    }

    IEnumerator MoveUP() {
        Vector3 pos = transform.position;
        Transform text = Instantiate(textObject, new Vector3(pos.x, pos.y + 0.3f, -2), Quaternion.identity).transform;
        pos = text.position;
        float a = 0;
        while (a < 1) {
            text.transform.position = Vector3.Lerp(pos, new Vector3(pos.x, pos.y + 0.5f, -2), a);
            a += 0.02f;
            yield return new WaitForFixedUpdate();
        }
        Destroy(text.gameObject, 1);
        gameObject.SetActive(false);
    }
}
