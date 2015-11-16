using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour {

    public int totalEnemySpeed = 20;
    public GameSetup setup;
    public AreaCapture areaCapture;

    void Start() {
        totalEnemySpeed = GameSetup.levelManager.getCurrentLevel().enemySpeed;
        // random number between 0 and 1
        double randomNumber = UnityEngine.Random.value;

        // distribute total force over x and y components 
        int xForce = (int)Math.Round(totalEnemySpeed * randomNumber);
        // needs sine/cosine math for consistent speed. 
        double angle = Math.Acos(((double)xForce) / ((double)totalEnemySpeed));
        int yForce = (int)Math.Round(1.0 * totalEnemySpeed * Math.Sin(angle));

        // randomize left/right and top/bottom
        if (UnityEngine.Random.value > 0.5)
            xForce = -xForce;
        if (UnityEngine.Random.value > 0.5)
            yForce = -yForce;
        // apply force
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
        // set a random rotation speed between -10 and +10
        GetComponent<Rigidbody2D>().angularVelocity = 10f * (2 * UnityEngine.Random.value - 1.0f);
    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        if(colInfo.collider.tag == "Player") {
            setup.gameOver();
        }
        if (areaCapture.colliderPartOfPath(colInfo.gameObject.GetComponent<BoxCollider2D>())){
            setup.gameOver();
        }
    }
}
