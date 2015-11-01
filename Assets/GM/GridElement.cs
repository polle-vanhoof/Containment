using UnityEngine;
using System.Collections;

public class GridElement : MonoBehaviour {

    public GameSetup setup;
    private bool captured = false;
    public ArrayList neighbours = new ArrayList();

    public void floodFill() {
        if (!captured) {
            GetComponent<Renderer>().enabled = true;

        }
    }

    public void addNeighbour(GridElement neighbour) {
        neighbours.Add(neighbour);
    }

}
