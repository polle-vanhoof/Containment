public class Level
{
    public int percentage { get; set; }
    public int nbOfMoves { get; set; }
    public float playerSpeed { get; set; }
    public int enemySpeed { get; set; }
    
    public Level(int percentage, int nbOfMoves)
    {
        playerSpeed = 5;
        enemySpeed = 20;
        this.percentage = percentage;
        this.nbOfMoves = nbOfMoves;
    }
    
}