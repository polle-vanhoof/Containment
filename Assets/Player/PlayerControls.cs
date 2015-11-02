using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

    public TrailRendererLevel trailRenderer;
    public AreaCapture areaCapture;

    public float speed = 5;
    public float sensitivity = 3;

    public bool onSide = true;
    public String direction = "right";
    private String wallSide = "";
    public bool lastXMovementLeft;
    public bool lastYMovementDown;

    public Rigidbody2D rb;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //Do not use moveRight here because this will mess up collisionDetection
        rb.velocity = new Vector2(speed, 0);
    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        // stop generating trail
        trailRenderer.stopTrail();

        // set wall side
        setWallSide();

        // create new captured area
        if (!onSide) {
            areaCapture.createCollisionIfRequired(true);
        }

        // make player follow the walls
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

        areaCapture.setLastMovePointNull();
    }

    private void moveDown()
    {
        rb.velocity = new Vector2(0, -speed);
        lastYMovementDown = true;
        direction = "down";
        if(onSide && wallSide == "bottom") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
    }

    private void moveUp()
    {
        rb.velocity = new Vector2(0, speed);
        lastYMovementDown = false;
        direction = "up";
        if (onSide && wallSide == "top") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
    }

    private void moveLeft()
    {
        rb.velocity = new Vector2(-speed, 0);
        lastXMovementLeft = true;
        direction = "left";
        if (onSide && wallSide == "left") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
    }

    private void moveRight()
    {
        rb.velocity = new Vector2(speed, 0);
        lastXMovementLeft = false;
        direction = "right";
        if (onSide && wallSide == "right") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
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

        if (Input.touchCount == 1) {
            Touch touch = Input.touches[0];
            if (Math.Abs(touch.deltaPosition.x) > Math.Abs(touch.deltaPosition.y)) {
                if (touch.deltaPosition.x > sensitivity) {
                    moveRight();
                } else if (touch.deltaPosition.x < -sensitivity) {
                    moveLeft();
                }
            } else {
                if (touch.deltaPosition.y > sensitivity) {
                    moveUp();
                } else if (touch.deltaPosition.y < -sensitivity) {
                    moveDown();
                }
            }
        }
    }


    private void setWallSide() {
        if (direction.Equals("up"))
            wallSide = "bottom";
        if (direction.Equals("down"))
            wallSide = "top";
        if (direction.Equals("left"))
            wallSide = "right";
        if (direction.Equals("right"))
            wallSide = "left";
    }


}
