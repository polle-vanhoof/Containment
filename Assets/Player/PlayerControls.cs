﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

    public TrailRendererLevel trailRenderer;
    public AreaCapture areaCapture;

    public float speed = 5;
    public float sensitivity = 3;

    public bool onSide = true;
    public String direction;
    private String wallSide = "bottom";
    private bool firstCollision = true;
    /*public bool lastXMovementLeft;
    public bool lastYMovementDown;*/

    public Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //Do not use moveRight here because this will mess up collisionDetection
        rb.velocity = new Vector2(speed, 0);
    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        if (firstCollision) {
            Debug.Log("first collision");
            firstCollision = false;
            return;
        }

        // determine the side of the player that collided
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        for(int i=0; i<colInfo.contacts.Length; i++) {
            Vector2 point = colInfo.contacts[i].point;
            if (point.x < minX)
                minX = point.x;
            if (point.x > maxX)
                maxX = point.x;
            if (point.y < minY)
                minY = point.y;
            if (point.y > maxY)
                maxY = point.y;
        }
        Vector2 playerCentre = new Vector2(transform.position.x, transform.position.y);
        Vector2 colpoint = new Vector2(minX + (maxX - minX) / 2f, minY + (maxY - minY) / 2f);
        Vector2 angle = playerCentre - colpoint;
        double angleX = Math.Round(angle.x,1);
        double angleY = Math.Round(angle.y, 1);
        String dir = "none";
        if(angleX == 0) {
            if(angleY == 0.1) {
                dir = "down";
            }else if(angleY == -0.1) {
                dir = "up";
            }
        }else if(angleY == 0) {
            if (angleX == 0.1) {
                dir = "left";
            } else if (angleX == -0.1) {
                dir = "right";
            }
        }
        if(angleX != 0 && angleY != 0) {
            dir = "err";
        }
        Debug.Log("collision!: " +dir);

        // if the player did not collide in the direction he was going => corner problem, do nothing
        if(direction != dir) {
            return;
        }

        // stop generating trail
        trailRenderer.stopTrail();
        String lastWallSide = wallSide;
        // set wall side
        setWallSide();
        //Debug.Log(wallSide);

        // create new captured area and move player in right direction
        if (!onSide) {
            if (GameSetup.debugMode) {
            Debug.Log("building wall");
            }
            areaCapture.createCollisionIfRequired(true);
        } else{
            if (lastWallSide == "bottom") {
                moveDown();
            }
            if (lastWallSide == "top") {
                moveUp();
            }
            if (lastWallSide == "left") {
                moveLeft();
            }
            if (lastWallSide == "right") {
                moveRight();
            }
        }

        if (colInfo.gameObject.name == "rightWall" ||
                colInfo.gameObject.name == "leftWall" ||
                colInfo.gameObject.name == "topWall" ||
                colInfo.gameObject.name == "bottomWall") {
            onSide = true;
        } else {
            foreach (BoxCollider2D collider in areaCapture.walls) {
                if (collider == colInfo.collider) {
                    onSide = true;
                }
            }
        }



        /*if (colInfo.gameObject.name == "rightWall" ||
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
            lastYMovementDown = false;*/

        areaCapture.setLastMovePointNull();
    }

    public void moveDown() {
        rb.velocity = new Vector2(0, -speed);
        //lastYMovementDown = true;
        direction = "down";
        if (onSide && wallSide == "bottom") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
    }

    public void moveUp() {
        rb.velocity = new Vector2(0, speed);
        //lastYMovementDown = false;
        direction = "up";
        if (onSide && wallSide == "top") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
    }

    public void moveLeft() {
        rb.velocity = new Vector2(-speed, 0);
        //lastXMovementLeft = true;
        direction = "left";
        if (onSide && wallSide == "left") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false);
    }

    public void moveRight() {
        rb.velocity = new Vector2(speed, 0);
        //lastXMovementLeft = false;
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

    public void setPlayerLocation(Vector2 position) {
        rb.position = position;
    }


}
