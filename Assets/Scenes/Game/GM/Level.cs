using UnityEngine;

public class Level {
    public int percentage { get; set; }
    public int nbOfMoves { get; set; }
    public float playerSpeed { get; set; }
    public int enemySpeed { get; set; }
    public string musicFileName { get; set; }
    public bool homing { get; set; }
    public bool isTutorial { get; set; }

    public Level(int percentage, int nbOfMoves, float playerSpeed, int enemySpeed, bool homing, string musicFileName) {
        this.playerSpeed = playerSpeed;
        this.enemySpeed = enemySpeed;
        this.percentage = percentage;
        this.nbOfMoves = nbOfMoves;
        this.musicFileName = musicFileName;
        this.homing = homing;
    }

    public Level(int percentage, int nbOfMoves, float playerSpeed, int enemySpeed, string musicFileName) {
        this.playerSpeed = playerSpeed;
        this.enemySpeed = enemySpeed;
        this.percentage = percentage;
        this.nbOfMoves = nbOfMoves;
        this.musicFileName = musicFileName;
        this.homing = false;
    }

}