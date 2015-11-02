﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AreaCapture : MonoBehaviour {

    public PlayerControls controls;
    private bool validLastMovePoint = false;
    private Vector2 lastMovePoint = Vector2.zero;
    public GameSetup setup;

    public Transform PrefabWall;
    public LinkedList<BoxCollider2D> walls = new LinkedList<BoxCollider2D>();

    void start() {
        walls.AddLast(setup.topWall);
        walls.AddLast(setup.bottomWall);
        walls.AddLast(setup.rightWall);
        walls.AddLast(setup.leftWall);
    }

    private void createCollisionBox(Vector2 point1, Vector2 point2, bool hitWall) {
        StartCoroutine(BuildDelayedCollisionBox(point1, point2, hitWall, 0.2f));
    }

    public void setLastMovePointNull() {
        lastMovePoint = Vector2.zero;
        validLastMovePoint = false;
    }

    public void setLastMovePoint(Vector2 point) {
        lastMovePoint = point;
        validLastMovePoint = true;
    }

    public void createCollisionIfRequired(bool hitWall) {
        if (!controls.onSide) {
            if (validLastMovePoint) {
                createCollisionBox(lastMovePoint, controls.rb.position, hitWall);
            }
            lastMovePoint = controls.rb.position;
            validLastMovePoint = true;
        }
    }

    IEnumerator BuildDelayedCollisionBox(Vector2 point1, Vector2 point2, bool hitWall, float delayTime) {
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
            walls.AddLast(newWall.GetComponent<BoxCollider2D>());
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
            walls.AddLast(newWall.GetComponent<BoxCollider2D>());
        }
        if (hitWall) {
        floodFill(((GameObject)((ArrayList)setup.sprites[0])[0]).GetComponent<GridElement>());
        }
    }

    public void floodFill(GridElement startElement) {
        Queue<GridElement> queue = new Queue<GridElement>();
        queue.Enqueue(startElement);
        while(queue.Count != 0) {
            GridElement element = queue.Dequeue();
            if (element.capture(walls)) {
                foreach (GridElement neighbour in element.getNeighbours()) {
                    queue.Enqueue(neighbour);
                }
            }
        }
    }

}
