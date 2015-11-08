using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameSetup : MonoBehaviour {

    public static bool debugMode = false;

    public Camera mainCam;
    public Rigidbody2D player;
    public Rigidbody2D enemy;
    public GameObject levelCompleteSprite;
    public GameObject gameOverSprite;
    public AreaCapture areaCapture;

    public BoxCollider2D topWall;
    public BoxCollider2D bottomWall;
    public BoxCollider2D rightWall;
    public BoxCollider2D leftWall;

    public GameObject gridSprite;
    public ArrayList sprites = new ArrayList();
    private Vector2 numSprites;
    public float spriteSize;

    public int numberOfGridElements;
    public float completionPercentage;

    // Update is called once per frame
    void Start() {
        // !!! fucks up all offsets - DO NOT USE !!!   => set in project settings instead
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        // set required capture percentage
        completionPercentage = 0.85f;

        // hide level complete sprite and game over sprite
        setUpLevelComplete();
        setUpGameOver();

        // Move each wall to its edge location
        setUpWalls();

        // Move player to its start location
        setUpPlayer();

        // Create game grid
        generateGrid();

    }

    private void setUpLevelComplete() {
        levelCompleteSprite.GetComponent<Renderer>().enabled = false;
        levelCompleteSprite.transform.position = new Vector2(0, 0);
        levelCompleteSprite.transform.localScale = new Vector2(2.5f, 2.5f);
    }

    private void setUpGameOver() {
        gameOverSprite.GetComponent<Renderer>().enabled = false;
        gameOverSprite.transform.position = new Vector2(0, 0);
        gameOverSprite.transform.localScale = new Vector2(3.5f, 3.5f);
    }

    private void generateGrid() {
        Vector2 bottomLeft = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector2 topRight = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        int matrixX = 0;
        for (float x = bottomLeft.x + spriteSize / 2f; x < topRight.x; x = x + spriteSize) {
            int matrixY = 0;
            ArrayList spritesLine = new ArrayList();
            for (float y = bottomLeft.y + spriteSize / 2f; y < topRight.y; y = y + spriteSize) {
                GameObject newSpriteObject = (GameObject)Instantiate(gridSprite, new Vector3(x, y, 0), Quaternion.identity);
                spritesLine.Add(newSpriteObject);
                newSpriteObject.GetComponent<Transform>().GetComponent<Renderer>().enabled = false;
                GridElement ownScript = newSpriteObject.GetComponent<GridElement>();
                if (matrixY > 0) {
                    GridElement leftNeighbour = ((GameObject)spritesLine[matrixY - 1]).GetComponent<GridElement>();
                    leftNeighbour.addNeighbour(ownScript);
                    ownScript.addNeighbour(leftNeighbour);
                }
                matrixY++;
            }
            sprites.Add(spritesLine);
            if (matrixX > 0) {
                ArrayList previousLine = (ArrayList)sprites[matrixX - 1];
                for (int i = 0; i < previousLine.Count; i++) {
                    GridElement element1 = ((GameObject)spritesLine[i]).GetComponent<GridElement>();
                    GridElement element2 = ((GameObject)previousLine[i]).GetComponent<GridElement>();
                    element1.addNeighbour(element2);
                    element2.addNeighbour(element1);
                }
            }
            matrixX++;
        }
        numSprites = new Vector2(sprites.Count, ((ArrayList)sprites[0]).Count);
        numberOfGridElements = (int)numSprites.x * (int)numSprites.y;
        Debug.Log("number of grid elements: " + numSprites.x + " x " + numSprites.y);
    }

    private void setUpPlayer() {
        // little bit to the right of top left corner
        float playerStartY = mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y;
        float playerStartX = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x + 10f;
        player.position = new Vector2(playerStartX, playerStartY);
    }


    private void setUpWalls() {
        topWall.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        topWall.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f);
        areaCapture.wallOrientation.Add(topWall, "H");

        bottomWall.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        bottomWall.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y - 0.5f);
        areaCapture.wallOrientation.Add(bottomWall, "H");

        rightWall.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        rightWall.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.6f, 0f);
        areaCapture.wallOrientation.Add(rightWall, "V");

        leftWall.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        leftWall.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);
        areaCapture.wallOrientation.Add(leftWall, "V");
    }


    public void gameOver() {
        player.velocity = new Vector2(0, 0);
        enemy.velocity = new Vector2(0, 0);
        enemy.GetComponent<Rigidbody2D>().angularVelocity = 0;
        gameOverSprite.GetComponent<Renderer>().enabled = true;
    }


    public void levelComplete() {
        player.velocity = new Vector2(0, 0);
        enemy.velocity = new Vector2(0, 0);
        enemy.GetComponent<Rigidbody2D>().angularVelocity = 0;
        levelCompleteSprite.GetComponent<Renderer>().enabled = true;
        
    }

    public GridElement findClosestGridElement(Vector2 point) {
        // Get a good estimate for the closest grid element

        // screen percentage
        float xPerc = mainCam.WorldToScreenPoint(new Vector3(point.x, point.y, 0f)).x / (Screen.width - (spriteSize / 2f));
        float yPerc = mainCam.WorldToScreenPoint(new Vector3(point.x, point.y, 0f)).y / (Screen.height - (spriteSize / 2f));

        int xEstimate = (int)Math.Round(sprites.Count * xPerc);
        int yEstimate = (int)Math.Round(((ArrayList)sprites[0]).Count * yPerc);

        // keep array within bounds
        if (xEstimate >= sprites.Count) {
            xEstimate = sprites.Count - 1;
        }
        if(xEstimate < 0) {
            xEstimate = 0;
        }
        if (yEstimate >= ((ArrayList)sprites[0]).Count) {
            yEstimate = ((ArrayList)sprites[0]).Count - 1;
        }
        if(yEstimate < 0) {
            yEstimate = 0;
        }

        ArrayList line = (ArrayList)sprites[xEstimate];
        GridElement estimate = ((GameObject)line[yEstimate]).GetComponent<GridElement>();

        float estimateDistance = Vector2.Distance(estimate.transform.position, point);
        bool foundClosest = false;

        int countSteps = 0;
        // move closer on x-axis
        while (!foundClosest) {
            int backupX = xEstimate;
            if (estimate.transform.position.x < point.x) {
                xEstimate++;
                if (xEstimate >= sprites.Count) {
                    xEstimate = sprites.Count - 1;
                }
            } else {
                xEstimate--;
                if (xEstimate < 0) {
                    xEstimate = 0;
                }
            }
            GridElement newEstimate = ((GameObject)((ArrayList)sprites[xEstimate])[yEstimate]).GetComponent<GridElement>();
            if (estimateDistance > Vector2.Distance(newEstimate.transform.position, point)) {
                estimate = newEstimate;
                estimateDistance = Vector2.Distance(newEstimate.transform.position, point);
                countSteps++;
            } else {
                foundClosest = true;
                xEstimate = backupX;
            }
        }

        foundClosest = false;

        // move closer on y-axis
        while (!foundClosest) {
            int backupY = yEstimate;
            if (estimate.transform.position.y < point.y) {
                yEstimate++;
                if (yEstimate >= ((ArrayList)sprites[0]).Count) {
                    yEstimate = ((ArrayList)sprites[0]).Count - 1;
                }
            } else {
                yEstimate--;
                if (yEstimate < 0) {
                    yEstimate = 0;
                }
            }
            GridElement newEstimate = ((GameObject)((ArrayList)sprites[xEstimate])[yEstimate]).GetComponent<GridElement>();
            if (estimateDistance > Vector2.Distance(newEstimate.transform.position, point)) {
                estimate = newEstimate;
                estimateDistance = Vector2.Distance(newEstimate.transform.position, point);
                countSteps++;
            } else {
                foundClosest = true;
                yEstimate = backupY;
            }
        }

        if (debugMode) {
            Debug.Log("CLOSEST GRID ELEMENT ------ point position: " + point + " --- element position: " + estimate.transform.position);
            Debug.Log("point x: " + point.x + " point y: " + point.y);
            Debug.Log("estimate x: " + estimate.transform.position.x + " estimate y: " + estimate.transform.position.y);
            Debug.Log("found closest grid element in " + countSteps + " steps");
        }

        return estimate;

        // inefficient way
      /*GridElement closest = ((GameObject)((ArrayList)sprites[0])[0]).GetComponent<GridElement>();
        for (int i = 0; i < sprites.Count; i++) {
            ArrayList spriteLine = (ArrayList)sprites[i];
            for (int j = 0; j < spriteLine.Count; j++) {
                GridElement element = ((GameObject)spriteLine[j]).GetComponent<GridElement>();
                if (Vector2.Distance(element.transform.position, point) < Vector2.Distance(closest.transform.position, point)) {
                    closest = element;
                }
            }
        }
        if (closest != estimate) {
            Debug.LogWarning("wrong element selected");
            Debug.Log("CLOSEST GRID ELEMENT ------ point position: " + point + " --- element position: " + estimate.transform.position);
            Debug.Log("correct x: " + point.x + " correct y: " + point.y);
            Debug.Log("estimate x: " + estimate.transform.position.x + " estimate y: " + estimate.transform.position.y);
            Debug.Log("correct distance: " + Vector2.Distance(closest.transform.position, point));
            Debug.Log("estimate distance: " + Vector2.Distance(estimate.transform.position, point));
        }
        return closest;*/
    }

    public void resetGridForEnemySearch() {
        for (int i = 0; i < sprites.Count; i++) {
            ArrayList spriteLine = (ArrayList)sprites[i];
            for (int j = 0; j < spriteLine.Count; j++) {
                GridElement element = ((GameObject)spriteLine[j]).GetComponent<GridElement>();
                element.resetEnemySearch();
            }
        }
    }


    void OnGUI() {
        if (debugMode) {
            string messageTop = "top wall: \n";
            messageTop += "x: " + topWall.size.x + "\n";
            messageTop += "y: " + topWall.size.y + "\n";
            messageTop += "offsetx: " + topWall.offset.x + "\n";
            messageTop += "offsety: " + topWall.offset.y + "\n";

            GUI.Label(new Rect(0, 0, 120, 100), messageTop);

            string messageBot = "bottom wall: \n";
            messageBot += "x: " + bottomWall.size.x + "\n";
            messageBot += "y: " + bottomWall.size.y + "\n";
            messageBot += "offsetx: " + bottomWall.offset.x + "\n";
            messageBot += "offsety: " + bottomWall.offset.y + "\n";

            GUI.Label(new Rect(130, 0, 120, 100), messageBot);

            string messageRight = "right wall: \n";
            messageRight += "x: " + rightWall.size.x + "\n";
            messageRight += "y: " + rightWall.size.y + "\n";
            messageRight += "offsetx: " + rightWall.offset.x + "\n";
            messageRight += "offsety: " + rightWall.offset.y + "\n";

            GUI.Label(new Rect(260, 0, 120, 100), messageRight);

            string messageLeft = "left wall: \n";
            messageLeft += "x: " + leftWall.size.x + "\n";
            messageLeft += "y: " + leftWall.size.y + "\n";
            messageLeft += "offsetx: " + leftWall.offset.x + "\n";
            messageLeft += "offsety: " + leftWall.offset.y + "\n";

            GUI.Label(new Rect(390, 0, 120, 100), messageLeft);
        }
    }
}
