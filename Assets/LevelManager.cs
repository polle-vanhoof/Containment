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
        levels.Add(new Level(50, 5));
        levels.Add(new Level(70, 5));
        levels.Add(new Level(50, 2));
        levels.Add(new Level(70, 2));
        levels.Add(new Level(85, 10));
        levels.Add(new Level(50, 1));
        levels.Add(new Level(80, 5));
        levels.Add(new Level(90, 10));
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
