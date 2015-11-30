using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour {

    public int totalEnemySpeed = 20;
    public GameSetup setup;
    public AreaCapture areaCapture;
    private bool playerHoming = false;
    private float homingModifier = 4;

    void Start() {
        totalEnemySpeed = GameSetup.getLevelManager().getCurrentLevel().enemySpeed;
        playerHoming = GameSetup.getLevelManager().getCurrentLevel().homing;

        double randomNumber = UnityEngine.Random.Range(0.4F, 0.6F); //Don't start vertical or horizontal because that's too easy!

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

        // not to the top right
        if(xForce > 0 && yForce > 0) {
            yForce = -yForce;
        }
        // apply force
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
        // set a random rotation speed between -10 and +10
        GetComponent<Rigidbody2D>().angularVelocity = 10f * (2 * UnityEngine.Random.value - 1.0f);
    }

    // Update is called once per frame
    void Update() {
        if (playerHoming) {
            GetComponent<Rigidbody2D>().AddForce((setup.player.transform.position - transform.position) * 5 * Time.smoothDeltaTime);
            if (GetComponent<Rigidbody2D>().velocity.magnitude * homingModifier  > totalEnemySpeed) {
                float scale = totalEnemySpeed / (GetComponent<Rigidbody2D>().velocity.magnitude * homingModifier);
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * scale, GetComponent<Rigidbody2D>().velocity.y * scale);
                Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D colInfo) {
        if(colInfo.collider.tag == "Player") {
            Debug.Log("hit player");
            setup.gameOver();
        }
        if (areaCapture.colliderPartOfPath(colInfo.gameObject.GetComponent<BoxCollider2D>())){
            Debug.Log("collision game over");
            setup.gameOver();
        }
    }
}
