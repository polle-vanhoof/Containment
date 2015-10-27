using UnityEngine;
using System.Collections;
using System;

public class PlayerControls : MonoBehaviour {

    public float speed = 5;
    public float sensitivity = 3;

    public bool onSide = true;
    public bool lastXMovementLeft;
    public bool lastYMovementDown;

    public Rigidbody2D rb;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //Do not use moveRight here because this will mess up collisionDetection
        rb.velocity = new Vector2(speed, 0);
    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        if (colInfo.gameObject.name == "rightWall" ||
                colInfo.gameObject.name == "leftWall" ||
                colInfo.gameObject.name == "topWall" ||
                colInfo.gameObject.name == "bottomWall")
            onSide = true;

        if (colInfo.gameObject.name == "rightWall" ||
                colInfo.gameObject.name == "leftWall")
            if (lastYMovementDown)
                moveDown();
            else
                moveUp();
        else if (colInfo.gameObject.name == "topWall" ||
                colInfo.gameObject.name == "bottomWall")
            if (lastXMovementLeft)
                moveLeft();
            else
                moveRight();

        if (colInfo.gameObject.name == "rightWall")
            lastXMovementLeft = true;
        if (colInfo.gameObject.name == "leftWall")
            lastXMovementLeft = false;
        if (colInfo.gameObject.name == "topWall")
            lastYMovementDown = true;
        if (colInfo.gameObject.name == "bottomWall")
            lastYMovementDown = false;
        /*if (lastYMovementDown && (colInfo.gameObject.name == "rightWall" ||
                colInfo.gameObject.name == "leftWall") )
            moveDown();
        else if (!lastYMovementDown && (colInfo.gameObject.name == "leftWall" && !lastYMovementDown)
            moveUp();
        else if (colInfo.gameObject.name == "topWall")
            moveRight();
        else if (colInfo.gameObject.name == "bottomWall")
            moveLeft();*/
    }

    private void moveDown()
    {
        rb.velocity = new Vector2(0, -speed);
        lastYMovementDown = true;
    }

    private void moveUp()
    {
        rb.velocity = new Vector2(0, speed);
        lastYMovementDown = false;
    }

    private void moveLeft()
    {
        rb.velocity = new Vector2(-speed, 0);
        lastXMovementLeft = true;
    }

    private void moveRight()
    {
        rb.velocity = new Vector2(speed, 0);
        lastXMovementLeft = false;
    }

    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            moveDown();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            moveUp();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            moveLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            moveRight();

        /*if (Input.touchCount == 1) {
            Touch touch = Input.touches[0];
            if (Math.Abs(touch.deltaPosition.x) > Math.Abs(touch.deltaPosition.y)) {
                if (touch.deltaPosition.x > sensitivity) {
                    rb.velocity = new Vector3(speed, 0);
                } else if (touch.deltaPosition.x < -sensitivity) {
                    rb.velocity = new Vector3(-speed, 0);
                }
            } else {
                if (touch.deltaPosition.y > sensitivity) {
                    rb.velocity = new Vector3(0, speed);
                } else if (touch.deltaPosition.y < -sensitivity) {
                    rb.velocity = new Vector3(0, -speed);
                }
            }
        }*/
    }
}
