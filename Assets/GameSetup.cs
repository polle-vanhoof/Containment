using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetup : MonoBehaviour {

    public static bool debugMode = false;

    public Camera mainCam;
    public Rigidbody2D player;

    public BoxCollider2D topWall;
    public BoxCollider2D bottomWall;
    public BoxCollider2D rightWall;
    public BoxCollider2D leftWall;

    // Update is called once per frame
    void Start() {
        // !!! fucks up all offsets - DO NOT USE !!!   => set in project settings instead
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Move each wall to its edge location

        topWall.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        topWall.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f);

        bottomWall.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
        bottomWall.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y - 0.5f);

        rightWall.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        rightWall.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.6f, 0f);

        leftWall.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
        leftWall.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);

        // little bit to the right of top left corner
        float playerStartY = mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y;
        float playerStartX = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x + 0.5f;
        player.position = new Vector2(playerStartX, playerStartY);

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


    public static void gameOver() {
        // game over
    }

    public static void captureArea(LinkedList<Vector2> cornerpoints) {
        // capture area based on player path corners
    }
}
