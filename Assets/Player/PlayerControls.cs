using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerControls : MonoBehaviour {

    public TrailRendererLevel trailRenderer;
    public AreaCapture areaCapture;
    public GameSetup setup;

    public float speed = 5;
    public float sensitivity = 3;

    public bool onSide = true;
    public String direction;
    private String wallSide = "bottom";
    private bool firstCollision = true;

    private String lastTouch = "";

    private bool cornerFollowMode = false;

    // NOT RELIABLE, DO NOT USE FOR ANYTHING OTHER THAN CORNER MOVEMENT EDGE CASES
    private Collider2D currentWall;
    private Collider2D badWall;
    private Collider2D earlyDetection;
    private Collider2D lastWall;
    // END NOT RELIABLE


    public Rigidbody2D rb;

    void Start() {
        speed = GameSetup.levelManager.getCurrentLevel().playerSpeed;
        rb = GetComponent<Rigidbody2D>();
        //Do not use moveRight here because this will mess up collisionDetection
        rb.velocity = new Vector2(speed, 0);
        RaycastHit2D wallTest = Physics2D.Raycast(rb.position, new Vector2(0, 1), Mathf.Infinity, (1 << 11));
        currentWall = wallTest.collider.gameObject.GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        if (firstCollision) {
            firstCollision = false;
            return;
        }

        if (colInfo.collider.tag == "Enemy") {
            return;
        }

        // reset early detection at every wall collision
        earlyDetection = null;

        // reset swipe lock
        lastTouch = "";

        // determine the side of the player that collided
        String dir = getCollisionSide(colInfo);
        if (GameSetup.debugMode) {
            Debug.Log("collision detected on player side: " + dir);
        }

        if (direction == dir && !areaCapture.colliderPartOfPath(colInfo.gameObject.GetComponent<BoxCollider2D>())) {
            if (GameSetup.debugMode)
                Debug.Log("setting current wall");
            currentWall = colInfo.gameObject.GetComponent<BoxCollider2D>();
        }

        // if the player did not collide in the direction he was going => corner problem, do nothing
        if (direction != dir) {
            badWall = colInfo.gameObject.GetComponent<BoxCollider2D>();
            if (GameSetup.debugMode) {
                Debug.Log("badwall");
            }
            return;
        }

        // stop generating trail
        if (!areaCapture.colliderPartOfPath(colInfo.gameObject.GetComponent<BoxCollider2D>())) {
            trailRenderer.stopTrail();
        }
        String lastWallSide = wallSide;
        // set wall side
        setWallSide();
        //Debug.Log(wallSide);

        // create new captured area and move player in right direction
        if (!onSide) {
            if (GameSetup.debugMode) {
                Debug.Log("building wall");
            }
            areaCapture.createCollisionIfRequired(true, colInfo.gameObject.GetComponent<BoxCollider2D>());
        } else {
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
            rb.position = (setup.findClosestGridElement(rb.position).transform.position);
        }

        if (colInfo.gameObject.name == "rightWall" ||
                colInfo.gameObject.name == "leftWall" ||
                colInfo.gameObject.name == "topWall" ||
                colInfo.gameObject.name == "bottomWall") {
            onSide = true;
        } else {
            if (!areaCapture.colliderPartOfPath(colInfo.gameObject.GetComponent<BoxCollider2D>())) {
                foreach (BoxCollider2D collider in areaCapture.walls) {
                    if (collider == colInfo.collider) {
                        onSide = true;
                    }
                }
            }
        }

        if (!areaCapture.colliderPartOfPath(colInfo.gameObject.GetComponent<BoxCollider2D>())) {
            areaCapture.setLastMovePointNull();
        }
    }

    private String getCollisionSide(Collision2D colInfo) {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        for (int i = 0; i < colInfo.contacts.Length; i++) {
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
        double angleX = Math.Round(angle.x, 1);
        double angleY = Math.Round(angle.y, 1);
        String dir = "none";
        if (angleX == 0) {
            if (angleY > 0) {
                dir = "down";
            } else if (angleY < 0) {
                dir = "up";
            }
        } else if (angleY == 0) {
            if (angleX > 0) {
                dir = "left";
            } else if (angleX < 0) {
                dir = "right";
            }
        }
        if (angleX != 0 && angleY != 0) {
            dir = "err";
        }

        if (dir == "err") {
            Debug.LogWarning("------Bad collision!------");
            Debug.LogWarning("number of collision points: " + colInfo.contacts.Length);
            Debug.LogWarning("collision point average: " + angle.x + " " + angle.y);
            Debug.LogWarning("point1: " + colInfo.contacts[0].point);
            Debug.LogWarning("point2: " + colInfo.contacts[1].point);
        }

        return dir;
    }

    public void moveDown() {
        if (!onSide && direction.Equals("up")) {
            return;
        }
        if (!cornerFollowMode && onSide && wallSide.Equals("top")) {
            return;
        }
        if (GameSetup.debugMode)
            Debug.Log("moving down");
        rb.velocity = new Vector2(0, -speed);
        direction = "down";
        if (onSide && wallSide == "bottom") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false, null);
    }

    public void moveUp() {
        if (!onSide && direction.Equals("down")) {
            return;
        }
        if (!cornerFollowMode && onSide && wallSide.Equals("bottom")) {
            return;
        }
        if (GameSetup.debugMode)
            Debug.Log("moving up");
        rb.velocity = new Vector2(0, speed);
        direction = "up";
        if (onSide && wallSide == "top") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false, null);
    }

    public void moveLeft() {
        if (!onSide && direction.Equals("right")) {
            return;
        }
        if (!cornerFollowMode && onSide && wallSide.Equals("right")) {
            return;
        }
        if (GameSetup.debugMode)
            Debug.Log("moving left");
        rb.velocity = new Vector2(-speed, 0);
        direction = "left";
        if (onSide && wallSide == "left") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false, null);
    }

    public void moveRight() {
        if (!onSide && direction.Equals("left")) {
            return;
        }
        if(!cornerFollowMode && onSide && wallSide.Equals("left")) {
            return;
        }
        if (GameSetup.debugMode)
            Debug.Log("moving right");
        rb.velocity = new Vector2(speed, 0);
        direction = "right";
        if (onSide && wallSide == "right") {
            onSide = false;
            trailRenderer.startTrail();
        }
        areaCapture.createCollisionIfRequired(false, null);
    }

    void FixedUpdate() {
        // reset early detection when moving manually
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            moveDown();
            earlyDetection = null;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            moveUp();
            earlyDetection = null;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            moveLeft();
            earlyDetection = null;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveRight();
            earlyDetection = null;
        }

        if (Input.touchCount == 1) {
            Touch touch = Input.touches[0];
            if (Math.Abs(touch.deltaPosition.x) > Math.Abs(touch.deltaPosition.y)) {
                if (touch.deltaPosition.x > sensitivity) {
                    if (!lastTouch.Equals("right")) {
                        moveRight();
                        lastTouch = "right";
                    }
                    earlyDetection = null;
                } else if (touch.deltaPosition.x < -sensitivity) {
                    if (!lastTouch.Equals("left")) {
                        moveLeft();
                        lastTouch = "left";
                    }
                    earlyDetection = null;
                }
            } else {
                if (touch.deltaPosition.y > sensitivity) {
                    if (!lastTouch.Equals("up")) {
                        moveUp();
                        lastTouch = "up";
                    }
                    earlyDetection = null;
                } else if (touch.deltaPosition.y < -sensitivity) {
                    if (!lastTouch.Equals("down")) {
                        moveDown();
                        lastTouch = "down";
                    }
                    earlyDetection = null;
                }
            }
        }

        // check if enemy hits path
        if (trailRenderer.GetComponent<TrailRenderer>().enabled == true) {
            if (areaCapture.isValidLastMovePoint()) {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, areaCapture.getLastMovePoint(), (1 << 10));
                Debug.DrawLine(transform.position, areaCapture.getLastMovePoint(), Color.white);
                if (hit.collider != null) {
                    if (hit.collider.gameObject.tag == "Enemy") {
                        setup.gameOver();
                    }
                }
            }
        }

        // check if you are about to go past a corner of a wall and act accordingly
        outerCornerFollow();

    }

    private void outerCornerFollow() {
        cornerFollowMode = true;
        if (onSide) {
            Vector2 rayDir = new Vector2(0, 1); // raycast towards wall you are following
            Vector2 rayOffset = new Vector2(0, 0);
            if (wallSide == "bottom") {
                rayDir = new Vector2(0, 1);
            }
            if (wallSide == "top") {
                rayDir = new Vector2(0, -1);
            }
            if (wallSide == "right") {
                rayDir = new Vector2(-1, 0);
            }
            if (wallSide == "left") {
                rayDir = new Vector2(1, 0);
            }
            Vector2 earlyDir = new Vector2(0, 1); // raycast to the back of player
            String rayWallAcceptOrientation = "";
            if (direction == "down") {
                earlyDir = new Vector2(0, 1);
                rayWallAcceptOrientation = "V";
            }
            if (direction == "up") {
                earlyDir = new Vector2(0, -1);
                rayWallAcceptOrientation = "V";
            }
            if (direction == "right") {
                earlyDir = new Vector2(-1, 0);
                rayWallAcceptOrientation = "H";
            }
            if (direction == "left") {
                earlyDir = new Vector2(1, 0);
                rayWallAcceptOrientation = "H";
            }
            RaycastHit2D wallTest = Physics2D.Raycast((rb.position), rayDir, Mathf.Infinity, (1 << 11));
            Debug.DrawRay(rb.position + rayOffset, (rayDir * 100), Color.green);
            Debug.DrawRay(rb.position, (earlyDir * 100), Color.blue);
            String colliderOrientation;
            if (wallTest.collider != earlyDetection) {
                areaCapture.wallOrientation.TryGetValue(wallTest.collider.gameObject.GetComponent<BoxCollider2D>(), out colliderOrientation);
                if (earlyDetection != null && rayWallAcceptOrientation.Equals(colliderOrientation)) {
                    earlyDetection = null;
                }

                if (wallTest.collider != null && wallTest.collider != lastWall && rayWallAcceptOrientation.Equals(colliderOrientation)) {
                    if (currentWall != wallTest.collider.gameObject.GetComponent<BoxCollider2D>() && badWall != wallTest.collider.gameObject.GetComponent<BoxCollider2D>()) {
                        if(GameSetup.debugMode) Debug.Log("different wall detected: " + wallTest.collider.gameObject.GetComponent<BoxCollider2D>().name);

                        // set player to exact grid position
                        // little bit hacky, move player to closest element, but slightly in the direction it will be going.
                        rb.position = (setup.findClosestGridElement(rb.position).transform.position + new Vector3(rayDir.x * 0.1f, rayDir.y * 0.1f, 0));
                        if (GameSetup.debugMode) Debug.Log("set player position: " + rb.position);

                        //avoid early detection
                        RaycastHit2D earlyWall = Physics2D.Raycast(rb.position, earlyDir, Mathf.Infinity, (1 << 11));
                        earlyDetection = earlyWall.collider.gameObject.GetComponent<BoxCollider2D>();
                        if (GameSetup.debugMode) Debug.Log("early detection: " + earlyDetection.name);

                        // move around corner
                        String lastDir = direction;
                        if (GameSetup.debugMode) Debug.Log("direction: " + lastDir);
                        if (wallSide == "bottom") {
                            moveUp();
                            if (lastDir == "left") {
                                wallSide = "left";
                            } else {
                                wallSide = "right";
                            }
                        } else if (wallSide == "top") {
                            moveDown();
                            if (lastDir == "left") {
                                wallSide = "left";
                            } else {
                                wallSide = "right";
                            }
                        } else if (wallSide == "right") {
                            moveLeft();
                            if (lastDir == "up") {
                                wallSide = "top";
                            } else {
                                wallSide = "bottom";
                            }
                        } else if (wallSide == "left") {
                            moveRight();
                            if (lastDir == "up") {
                                wallSide = "top";
                            } else {
                                wallSide = "bottom";
                            }
                        }
                        lastWall = currentWall;
                        currentWall = badWall;
                        badWall = null;
                    }
                }
            }
        }
        cornerFollowMode = false;
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

    public void setCurrentWall(Collider2D wall) {
        this.currentWall = wall;
        this.badWall = null;
    }


}
