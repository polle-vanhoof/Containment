using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AreaCapture : MonoBehaviour {

    public EnemyAI enemy;
    public PlayerControls controls;
    private bool validLastMovePoint = false;
    private Vector2 lastMovePoint = Vector2.zero;
    public GameSetup setup;

    private int gridElementsCaptured;
    public Transform PrefabWall;
    public LinkedList<BoxCollider2D> walls = new LinkedList<BoxCollider2D>();
    public Dictionary<BoxCollider2D, String> wallOrientation = new Dictionary<BoxCollider2D, string>();
    public LinkedList<BoxCollider2D> Pathwalls = new LinkedList<BoxCollider2D>();
    public LinkedList<GridElement> additionalFillPoints = new LinkedList<GridElement>();

    void OnGUI() {
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(0, 0, 120, 100), "" + walls.Count);
    }

    void start() {
        walls.AddLast(setup.topWall);
        walls.AddLast(setup.bottomWall);
        walls.AddLast(setup.rightWall);
        walls.AddLast(setup.leftWall);
    }

    private void createCollisionBox(Vector2 point1, Vector2 point2, bool hitWall, BoxCollider2D targetWall) {
        StartCoroutine(BuildDelayedCollisionBox(point1, point2, hitWall, targetWall, 0.1f));
    }

    public void setLastMovePointNull() {
        lastMovePoint = Vector2.zero;
        validLastMovePoint = false;
    }

    private void setLastMovePoint(Vector2 point) {
        lastMovePoint = point;
        validLastMovePoint = true;
    }

    public Vector2 getLastMovePoint() {
        return lastMovePoint;
    }

    public bool isValidLastMovePoint() {
        return validLastMovePoint;
    }

    public void createCollisionIfRequired(bool hitWall, BoxCollider2D targetWall) {
        if (!controls.onSide) {
            if (validLastMovePoint) {
                createCollisionBox(lastMovePoint, controls.rb.position, hitWall, targetWall);
            }
            setLastMovePoint(controls.rb.position);
        }
    }

    IEnumerator BuildDelayedCollisionBox(Vector2 point1, Vector2 point2, bool hitWall, BoxCollider2D targetWall, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        GridElement e1 = setup.findClosestGridElement(point1);
        GridElement e2 = setup.findClosestGridElement(point2);
        point1 = new Vector2(e1.transform.position.x, e1.transform.position.y);
        point2 = new Vector2(e2.transform.position.x, e2.transform.position.y);

        if (GameSetup.debugMode) {
            Debug.Log("Collision Box between (" + point1.x + "," + point1.y + ") and (" + point2.x + "," + point2.y + ")");
        }
        Vector2 floodFillStartPoint;
        if (point1.x == point2.x) {
            Transform newWall = (Transform)Instantiate(PrefabWall, new Vector3(0, 0, 0), Quaternion.identity);
            newWall.name = newWall.name + " " + (walls.Count+1);
            newWall.GetComponent<BoxCollider2D>().size = new Vector2(setup.spriteSize, Math.Abs(point1.y - point2.y) + setup.spriteSize);
            float yOffset;
            if (point1.y < point2.y) {
                yOffset = point1.y + Math.Abs(point1.y - point2.y) / 2.0f;
            } else {
                yOffset = point2.y + Math.Abs(point1.y - point2.y) / 2.0f;
            }
            newWall.GetComponent<BoxCollider2D>().offset = new Vector2(point1.x, yOffset);
            walls.AddLast(newWall.GetComponent<BoxCollider2D>());
            wallOrientation.Add(newWall.GetComponent<BoxCollider2D>(), "V");
            Pathwalls.AddLast(newWall.GetComponent<BoxCollider2D>());
            float backOffset = setup.spriteSize + 0.05f;
            if (controls.direction.Equals("up")) {
                backOffset = -backOffset;
            }
            floodFillStartPoint = new Vector2(point1.x + setup.spriteSize, point2.y + backOffset);
            if (hitWall) {
                Vector2 newPlayerloc;
                if (TestForEnemy(setup.findClosestGridElement(floodFillStartPoint))) {
                    newPlayerloc = new Vector2(point2.x + (setup.spriteSize + 0.15f), point2.y);
                    controls.setPlayerLocation(newPlayerloc);
                    controls.moveRight();
                    floodFillStartPoint = new Vector2(point1.x - setup.spriteSize, point2.y + backOffset);
                } else {
                    newPlayerloc = new Vector2(point2.x - setup.spriteSize, point2.y);
                    controls.setPlayerLocation(newPlayerloc);
                    controls.moveLeft();
                }
            }
            if (colliderPartOfPath(targetWall)) {
                if (TestForEnemy(setup.findClosestGridElement(floodFillStartPoint))) {
                    GridElement element = setup.findClosestGridElement(new Vector2(point1.x - setup.spriteSize, point2.y + backOffset));
                    additionalFillPoints.AddLast(element);
                } else {
                    GridElement element = setup.findClosestGridElement(floodFillStartPoint);
                    additionalFillPoints.AddLast(element);
                }
            }
        } else {
            Transform newWall = (Transform)Instantiate(PrefabWall, new Vector3(0, 0, 0), Quaternion.identity);
            newWall.name = newWall.name + " " + (walls.Count+1);
            newWall.GetComponent<BoxCollider2D>().size = new Vector2(Math.Abs(point1.x - point2.x) + setup.spriteSize, setup.spriteSize);
            float xOffset;
            if (point1.x < point2.x) {
                xOffset = point1.x + Math.Abs(point1.x - point2.x) / 2.0f; ;
            } else {
                xOffset = point2.x + Math.Abs(point1.x - point2.x) / 2.0f; ;
            }
            newWall.GetComponent<BoxCollider2D>().offset = new Vector2(xOffset, point1.y);
            walls.AddLast(newWall.GetComponent<BoxCollider2D>());
            wallOrientation.Add(newWall.GetComponent<BoxCollider2D>(), "H");
            Pathwalls.AddLast(newWall.GetComponent<BoxCollider2D>());
            float backOffset = setup.spriteSize + 0.05f;
            if (controls.direction.Equals("right")) {
                backOffset = -backOffset;
            }
            floodFillStartPoint = new Vector2(point2.x + backOffset, point1.y + setup.spriteSize);
            if (hitWall) {
                Vector2 newPlayerLoc;
                if (TestForEnemy(setup.findClosestGridElement(floodFillStartPoint))) {
                    newPlayerLoc = new Vector2(point2.x, point2.y + (setup.spriteSize + 0.15f));
                    controls.setPlayerLocation(newPlayerLoc);
                    controls.moveUp();
                    floodFillStartPoint = new Vector2(point2.x + backOffset, point1.y - setup.spriteSize);
                } else {
                    newPlayerLoc = new Vector2(point2.x, point2.y - setup.spriteSize);
                    controls.setPlayerLocation(newPlayerLoc);
                    controls.moveDown();
                }
            }
            if (colliderPartOfPath(targetWall)) {
                if (TestForEnemy(setup.findClosestGridElement(floodFillStartPoint))) {
                    GridElement element = setup.findClosestGridElement(new Vector2(point1.x - setup.spriteSize, point2.y + backOffset));
                    additionalFillPoints.AddLast(element);
                } else {
                    GridElement element = setup.findClosestGridElement(floodFillStartPoint);
                    additionalFillPoints.AddLast(element);
                }
            }
        }
        if (hitWall && !colliderPartOfPath(targetWall)) {
            Pathwalls.Clear();
            GridElement closestPoint = setup.findClosestGridElement(floodFillStartPoint);
            floodFill(closestPoint);
        }
    }

    private void floodFill(GridElement startElement) {
        GridElement wallElement = null;
        Queue<GridElement> queue = new Queue<GridElement>();
        queue.Enqueue(startElement);

        // add additional fillpoints for special cases
        foreach (GridElement point in additionalFillPoints) {
            queue.Enqueue(point);
        }

        int count = 0;
        while (queue.Count != 0) {
            GridElement element = queue.Dequeue();
            if (element.capture(walls)) {
                count++;
                foreach (GridElement neighbour in element.getNeighbours()) {
                    queue.Enqueue(neighbour);
                }
            } else if (wallElement == null) {
                if (element.ContainsWall(walls) && !element.iscaptured()) {
                    wallElement = element;
                }
            }
        }

        // floodfill the wall as well
        queue.Enqueue(wallElement);
        while (queue.Count != 0) {
            count++;
            GridElement element = queue.Dequeue();
            if (element != null && element.captureWall(walls)) {
                count++;
                foreach (GridElement neighbour in element.getNeighbours()) {
                    queue.Enqueue(neighbour);
                }
            }
        }

        additionalFillPoints.Clear();
        gridElementsCaptured += count;

        float percentageCaptured = (gridElementsCaptured * 1.0f) / (setup.numberOfGridElements*1.0f);
        if (percentageCaptured > setup.completionPercentage) {
            setup.levelComplete();
        }

        if (GameSetup.debugMode) {
            Debug.Log("filling " + count + " grid elements");
        }
    }

    private bool TestForEnemy(GridElement startElement) {
        setup.resetGridForEnemySearch();
        bool containsEnemy = false;
        Queue<GridElement> queue = new Queue<GridElement>();
        queue.Enqueue(startElement);
        while (queue.Count != 0) {
            GridElement element = queue.Dequeue();
            if (element.findEnemy(walls)) {
                if (element.ContainsEnemy(enemy)) {
                    containsEnemy = true;
                    break;
                }
                foreach (GridElement neighbour in element.getNeighbours()) {
                    queue.Enqueue(neighbour);
                }
            }
        }
        return containsEnemy;
    }

    public bool colliderPartOfPath(BoxCollider2D collider) {
        if(collider == null) {
            return false;
        }
        if (Pathwalls.Contains(collider)) {
            return true;
        }
        return false;
    }

    public void resetCapturePoints() {
        gridElementsCaptured = 0;
    }

}
