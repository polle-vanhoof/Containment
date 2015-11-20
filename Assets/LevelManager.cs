using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

    public List<Level> levels { get; set; }

    int _currentLevelIndex;
    public int currentLevelIndex {
        set
        {
            if (value < levels.Count)
                _currentLevelIndex = value;
        }
        get { return _currentLevelIndex; } }

    public int unlockedLevelIndex { set; get; }

    public LevelManager()
    {
        levels = new List<Level>();
        levels.Add(new Level(65, 20, 7, 25, "Sounds/level1"));
        //http://freesound.org/people/mickleness/sounds/316975/
        levels.Add(new Level(65, 20, 7, 35, "Sounds/level2"));
        //http://freesound.org/people/mickleness/sounds/244004/
        levels.Add(new Level(65, 20, 4, 25, "Sounds/level3"));
        unlockedLevelIndex = levels.Count-1;
        currentLevelIndex = unlockedLevelIndex;
    }

    public Level getCurrentLevel()
    {
        return levels[currentLevelIndex];
    }

    public bool isLastLevel()
    {
        return currentLevelIndex == levels.Count - 1;
    }
}
