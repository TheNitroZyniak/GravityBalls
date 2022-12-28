using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour{
    Vector2 startPos, endPos;
    

    public void SetBlocks(Transform block) {
        block.parent = transform;
    }

    public IEnumerator MoveUp() {
        startPos = transform.position;
        endPos = new Vector2(0, transform.position.y + 0.75f);
        float changeAlpha = 0.05f;

        float a = 0;
        while (a < 1) {
            transform.position = Vector2.Lerp(startPos, endPos, a);
            a += changeAlpha;

            changeAlpha -= 0.0012f;

            yield return new WaitForFixedUpdate();
        }
        transform.position = endPos;
        for (int i = 0; i < GameManager.figures.Count; i++) {
            if (GameManager.figures[i].positionY == 8) {
                StartCoroutine(GameManager.figures[i].Shake());
            }
        }
    }
}