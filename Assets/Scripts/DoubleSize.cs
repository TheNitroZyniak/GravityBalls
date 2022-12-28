using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSize : MonoBehaviour{
    [SerializeField] GameObject textObject;
    public int positionY;

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Ball>().Expand();


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
            a += 0.025f;
            yield return new WaitForFixedUpdate();
        }
        Destroy(text.gameObject, 1);
        gameObject.SetActive(false);
    }
}
