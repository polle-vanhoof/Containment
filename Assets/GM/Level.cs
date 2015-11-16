using UnityEngine;

public class Level
{
    public int percentage { get; set; }
    public int nbOfMoves { get; set; }
    public float playerSpeed { get; set; }
    public int enemySpeed { get; set; }
    public AudioClip music;
    
    public Level(int percentage, int nbOfMoves, float playerSpeed, int enemySpeed, string audioFile)
    {
        this.playerSpeed = playerSpeed;
        this.enemySpeed = enemySpeed;
        this.percentage = percentage;
        this.nbOfMoves = nbOfMoves;
        music = (AudioClip)Resources.Load(audioFile, typeof(AudioClip));
    }
    
}