using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AreaCapture : MonoBehaviour {

    public PlayerControls controls;
    private bool validLastMovePoint = false;
    private Vector2 lastMovePoint = Vector2.zero;

    public Transform PrefabWall;

    private void createCollisionBox(Vector2 point1, Vector2 point2) {
        StartCoroutine(BuildDelayedCollisionBox(point1, point2, 0.2f));
    }

    public void setLastMovePointNull() {
        lastMovePoint = Vector2.zero;
        validLastMovePoint = false;
    }

    public void setLastMovePoint(Vector2 point) {
        lastMovePoint = point;
        validLastMovePoint = true;
    }

    public void createCollisionIfRequired() {
        if (!controls.onSide) {
            if (validLastMovePoint) {
                createCollisionBox(lastMovePoint, controls.rb.position);
            }
            lastMovePoint = controls.rb.position;
            validLastMovePoint = true;
        }
    }

    IEnumerator BuildDelayedCollisionBox(Vector2 point1, Vector2 point2, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Collision Box between (" + point1.x + "," + point1.y + ") and (" + point2.x + "," + point2.y + ")");
        if (point1.x == point2.x) {
            Transform newWall = (Transform)Instantiate(PrefabWall, new Vector3(0, 0, 0), Quaternion.identity);
            newWall.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, Math.Abs(point1.y - point2.y));
            float yOffset;
            if (point1.y < point2.y) {
                yOffset = point1.y + Math.Abs(point1.y - point2.y) / 2.0f;
            } else {
                yOffset = point2.y + Math.Abs(point1.y - point2.y) / 2.0f;
            }
            newWall.GetComponent<BoxCollider2D>().offset = new Vector2(point1.x, yOffset);
            /*
            BoxCollider2D collider = new BoxCollider2D();
            collider.size = new Vector2(0.1f, Math.Abs(point1.y - point2.y));
            collider.offset = new Vector2(point1.x, Math.Abs(point1.y - point2.y) / 2f);*/
        } else {
            Transform newWall = (Transform)Instantiate(PrefabWall, new Vector3(0, 0, 0), Quaternion.identity);
            newWall.GetComponent<BoxCollider2D>().size = new Vector2(Math.Abs(point1.x - point2.x), 0.1f);
            float xOffset;
            if(point1.x < point2.x) {
                xOffset = point1.x + Math.Abs(point1.x - point2.x) / 2.0f; ;
            } else {
                xOffset = point2.x + Math.Abs(point1.x - point2.x) / 2.0f; ;
            }
            newWall.GetComponent<BoxCollider2D>().offset = new Vector2(xOffset, point1.y);

            /*
            BoxCollider2D collider = new BoxCollider2D();
            collider.size = new Vector2(Math.Abs(point1.x - point2.x), 0.1f);
            collider.offset = new Vector2(Math.Abs(point1.x - point2.x) / 2f, point1.y);*/
        }
    }

}
