using UnityEngine;
using System.Collections;
using System;

public class PlayerControls : MonoBehaviour {
    public float speed = 5;
    public float sensitivity = 3;

    public Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (Input.touchCount == 1) {
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
        } else {
            rb.velocity = new Vector3(0, 0);
        }
    }
}
