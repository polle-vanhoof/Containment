using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MainMenuSetup : MonoBehaviour {

    public GoogleAnalyticsV3 googleAnalytics;

    public Camera cam;
    public Canvas canvas;
    public Canvas buttonCanvas;

    public SpriteRenderer backgroundSprite;

    //usefull screenpoints
    private Vector2 bottomLeft;
    private Vector2 topRight;

	// Use this for initialization
	void Start () {

        PlayerPrefs.SetInt("playerId", UnityEngine.Random.Range(1, Int32.MaxValue));
        MusicScript.music.play("Sounds/MenuMusic");
        // Get screen positions en calculate usefull points
        bottomLeft = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        screenfitBackground();
        int playerId = PlayerPrefs.GetInt("playerId");
        googleAnalytics.LogScreen("Player: " + playerId + ", Main Menu");
	}


    private void screenfitBackground() {
        Sprite sp = backgroundSprite.sprite;
        Vector3 pos = transform.position;
        Vector3[] array = new Vector3[2];
        //top left
        array[0] = pos + sp.bounds.min;
        // Bottom right
        array[1] = pos + sp.bounds.max;

        float width = Math.Abs(array[1].x - array[0].x);
        float height = Math.Abs(array[0].y - array[1].y);

        float screenWidth = topRight.x - bottomLeft.x;
        float screenHeight = topRight.y - bottomLeft.y;

        float scaleX = screenWidth / width;
        float scaleY = screenHeight / height;

        Debug.Log("sp: " + width + "   " + height);
        Debug.Log("screen: " + screenWidth + "    " + screenHeight);
        Debug.Log("scale: " + scaleX + "    " + scaleY);

        backgroundSprite.transform.localScale = new Vector2(scaleX, scaleY);
    }

}
