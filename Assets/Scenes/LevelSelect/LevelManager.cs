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
        levels.Add(new Level(65, 20, 7, 25, "Sounds/music1"));
        levels.Add(new Level(75, 20, 7, 25, "Sounds/music1"));
        levels.Add(new Level(85, 20, 7, 25, "Sounds/music1"));

        //http://freesound.org/people/mickleness/sounds/316975/
        levels.Add(new Level(65, 20, 7, 25, "Sounds/music2"));
        levels.Add(new Level(65, 6, 7, 25, "Sounds/music2"));
        levels.Add(new Level(65, 3, 7, 25, "Sounds/music2"));

        //http://freesound.org/people/mickleness/sounds/244004/
        levels.Add(new Level(70, 10, 6, 25, "Sounds/music3"));
        levels.Add(new Level(70, 10, 5, 25, "Sounds/music3"));
        levels.Add(new Level(70, 10, 4, 25, "Sounds/music3"));

        // http://ericskiff.com/music/
        levels.Add(new Level(70, 10, 6, 30, "Sounds/music4"));
        levels.Add(new Level(70, 10, 5, 35, "Sounds/music4"));
        levels.Add(new Level(70, 10, 4, 40, "Sounds/music4"));

        // https://soundcloud.com/ddddan/ddddan-bassss
        levels.Add(new Level(85, 5, 7, 30, "Sounds/music4"));
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
