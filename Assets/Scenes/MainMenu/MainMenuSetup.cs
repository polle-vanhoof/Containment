using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainSetup : MonoBehaviour {

    public GameObject titleBar;
    public Camera cam;
    public Canvas canvas;
    public Canvas buttonCanvas;

    public GameObject playButton;
    public GameObject levelSelectButton;

    //usefull screenpoints
    private Vector2 bottomLeft;
    private Vector2 topRight;
    private Vector2 screenCenter;
    private float screenWidth;
    private float screenHeight;

	// Use this for initialization
	void Start () {
        // Get screen positions en calculate usefull points
        bottomLeft = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        screenCenter = new Vector2((topRight.x + bottomLeft.x) / 2, (topRight.y + bottomLeft.y) / 2);
        screenWidth = topRight.x - bottomLeft.x;
        screenHeight = topRight.y - bottomLeft.y;

        // Fit title to screen
        screenfitTitle();

        // Fit buttons to screen
        screenfitButtons();
	}


    private void screenfitTitle() {
        // Get title position
        RectTransform title = titleBar.GetComponent<RectTransform>();

        // Get title size
        Vector2 size = title.rect.size;


        LineRenderer line = titleBar.GetComponent<LineRenderer>();
        Vector2 linePos1 = new Vector2(-size.x / 2.1f, -size.y / 3f);
        Vector2 linePos2 = new Vector2(size.x / 2.1f, -size.y / 3f);
        line.SetPosition(0, linePos1);
        line.SetPosition(1, linePos2);
    }


    private void screenfitButtons() {
        // fuck this, used canvas scaling instead

        /*// Set button canvas position
        buttonCanvas.transform.position = new Vector2(screenCenter.x, screenCenter.y - screenHeight / 8);

        // Set button canvas scale
        RectTransform buttonRect = buttonCanvas.GetComponent<RectTransform>();
        Vector2 size = buttonRect.rect.size;

        Debug.Log(size.x / Screen.width + "   " + size.y / Screen.height);
        Debug.Log(Screen.width + "    " + Screen.height);*/
    }

}
