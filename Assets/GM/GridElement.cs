using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridElement : MonoBehaviour {

    public AreaCapture areaCaptureScript;
    private bool captured = false;
    private bool searchedForEnemy = false;
    public ArrayList neighbours = new ArrayList();

    public ArrayList getNeighbours() {
        return neighbours;
    }

    public bool capture(LinkedList<BoxCollider2D> walls) {
        if (captured || ContainsWall(walls)) {
            return false;
        } else {
            GetComponent<Renderer>().enabled = true;
            captured = true;
            return true;
        }
    }

    public bool captureWall(LinkedList<BoxCollider2D> walls) {
        if (captured || !ContainsWall(walls)) {
            return false;
        } else {
            GetComponent<Renderer>().enabled = true;
            captured = true;
            return true;
        }
    }

    public void addNeighbour(GridElement neighbour) {
        neighbours.Add(neighbour);
    }


    public bool ContainsWall(LinkedList<BoxCollider2D> walls) {
        bool containsWall = false;

        foreach (BoxCollider2D box in walls) {
            if (box.bounds.Contains(new Vector2(transform.position.x + 0.02f, transform.position.y))) {
                containsWall = true;
            }
            if (box.bounds.Contains(new Vector2(transform.position.x - 0.02f, transform.position.y))) {
                containsWall = true;
            }
            if (box.bounds.Contains(new Vector2(transform.position.x, transform.position.y + 0.02f))) {
                containsWall = true;
            }
            if (box.bounds.Contains(new Vector2(transform.position.x + 0.05f, transform.position.y - 0.02f))) {
                containsWall = true;
            }
        }
        return containsWall;
    }


    public bool ContainsEnemy(EnemyAI enemy) {
        bool containsEnemy = false;
        BoxCollider2D box = enemy.gameObject.GetComponent<BoxCollider2D>();
        if (box.bounds.Contains(transform.position)) {
            Debug.Log("enemy found");
            containsEnemy = true;
        }
        return containsEnemy;
    }

    public bool findEnemy(LinkedList<BoxCollider2D> walls) {
        if (searchedForEnemy || ContainsWall(walls)) {
            return false;
        } else {
            searchedForEnemy = true;
            return true;
        }
    }

    public void resetEnemySearch() {
        searchedForEnemy = false;
    }
}
