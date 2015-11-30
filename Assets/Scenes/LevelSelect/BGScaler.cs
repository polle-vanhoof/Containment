using UnityEngine;
using System.Collections;
using System;

public class BGScaler : MonoBehaviour {

    public Camera cam;

    public SpriteRenderer backgroundSprite;

    //usefull screenpoints
    private Vector2 bottomLeft;
    private Vector2 topRight;

    // Use this for initialization
    void Start() {
        // Get screen positions en calculate usefull points
        bottomLeft = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        screenfitBackground();
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

        backgroundSprite.transform.localScale = new Vector2(scaleX, scaleY);
    }
}
