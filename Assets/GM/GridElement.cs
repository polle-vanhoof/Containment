using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridElement : MonoBehaviour {

    private bool captured = false;
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

    public void addNeighbour(GridElement neighbour) {
        neighbours.Add(neighbour);
    }


    public bool ContainsWall(LinkedList<BoxCollider2D> walls) {
        bool isOverlapped = false;

        foreach (BoxCollider2D box in walls) {
            if (box.bounds.Contains(new Vector2(transform.position.x +0.02f, transform.position.y))) {
                isOverlapped = true;
            }
            if (box.bounds.Contains(new Vector2(transform.position.x - 0.02f, transform.position.y))) {
                isOverlapped = true;
            }
            if (box.bounds.Contains(new Vector2(transform.position.x , transform.position.y + 0.02f))) {
                isOverlapped = true;
            }
            if (box.bounds.Contains(new Vector2(transform.position.x + 0.05f, transform.position.y -0.02f))) {
                isOverlapped = true;
            }
        }
        return isOverlapped;
    }

}
