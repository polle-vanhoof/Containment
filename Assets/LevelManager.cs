using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

    public List<Level> levels { get; set; }
    public int currentLevelIndex { set; get; }
    public int unlockedLevelIndex { set; get; }

    public LevelManager()
    {
        levels = new List<Level>();
        levels.Add(new Level(50, 10));
        levels.Add(new Level(75, 10));
        levels.Add(new Level(80, 5));
        unlockedLevelIndex = levels.Count-1;
        currentLevelIndex = unlockedLevelIndex;
    }

    public Level getCurrentLevel()
    {
        return levels[currentLevelIndex];
    }

}
