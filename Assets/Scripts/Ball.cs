using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] TrailRenderer tr;

    float speed = 8;
    PhysicsMaterial2D pm2D;
    Vector3 size;
    public bool isBig, isFlying;

    Vector2 downVel;
    bool stable;

    private void Start() {
        transform.localScale = transform.localScale * GameManager.screenRatio;
        size = transform.localScale;
        rb.gravityScale = 4;
        downVel = new Vector2(0.0001f, 0.0001f);
        GameManager.balls.Add(gameObject.GetComponent<Ball>());
    }

    private void Update() {
        if(transform.position.y < -3.7f && pm2D.bounciness == 1) {
            tr.enabled = false;
            pm2D.bounciness = 0;
            rb.sharedMaterial = pm2D;
        }

        if(transform.position.y < -5.125f) {
            transform.position = new Vector3(transform.position.x, 5.125f, transform.position.z);
            for (int j = 0; j < GameManager.balls.Count; j++) {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameManager.balls[j].GetComponent<Collider2D>(), false);
            }
            GameManager.ballsCount++;
            rb.gravityScale = 4;
        }
        if (stable) {
            if (Mathf.Abs(rb.velocity.x) < downVel.x && Mathf.Abs(rb.velocity.y) < downVel.y && isFlying) {
                transform.position = new Vector3(0, -4, transform.position.z);
            }
        }

    }

    public void Push(Vector2 direction) {
        float a = 0;
        Vector3 startPos = transform.position;
        transform.position = new Vector3(0, 3.25f, 0);
        
        for (int j = 0; j < GameManager.balls.Count; j++) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameManager.balls[j].GetComponent<Collider2D>());
        }
        rb.velocity = direction.normalized * speed;
        if (pm2D == null) {
            pm2D = new PhysicsMaterial2D();
        }
        rb.gravityScale = 0.05f;
        pm2D.bounciness = 1;
        rb.sharedMaterial = pm2D;
        isFlying = true;
    }

    public void SetParams() {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        tr.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("BottomBorder")) {
            rb.velocity = new Vector2(-Mathf.Sign(transform.position.x) * 5, -5);
        }
        if (collision.gameObject.CompareTag("SideBorder")) {
            rb.angularVelocity = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Figure"))
            stable = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Figure")) {
            rb.gravityScale = 2.5f;
            rb.velocity = rb.velocity.normalized * speed;
            UI.score++;
        }
        stable = false;
    }

    public void SetPM2D() {
        if (pm2D == null) {
            pm2D = new PhysicsMaterial2D();
        }
        pm2D.bounciness = 1;
        rb.sharedMaterial = pm2D;
    }

    public void Expand() {
        transform.localScale = size * 1.2f;
        tr.startWidth = size.x * 1.2f;
        isBig = true;
    }
}