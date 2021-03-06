﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

    public List<Level> levels { get; set; }

    int _currentLevelIndex;
    public int currentLevelIndex {
        set {
            if (value < levels.Count)
                _currentLevelIndex = value;
        }
        get { return _currentLevelIndex; }
    }

    

    public LevelManager() {
        levels = new List<Level>();
        Level lvl1 = new Level(65, 20, 7, 25, "Sounds/music1");
        lvl1.isTutorial = true;
        levels.Add(lvl1);
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
        levels.Add(new Level(70, 10, 7, 30, "Sounds/music4"));
        levels.Add(new Level(70, 10, 7, 35, "Sounds/music4"));
        levels.Add(new Level(70, 10, 7, 40, "Sounds/music4"));

        // https://soundcloud.com/ddddan/ddddan-bassss
        levels.Add(new Level(65, 20, 7, 25, true, "Sounds/music5"));
        levels.Add(new Level(75, 20, 7, 25, true, "Sounds/music5"));
        levels.Add(new Level(85, 20, 7, 25, true, "Sounds/music5"));

        levels.Add(new Level(65, 20, 7, 25, true, "Sounds/music5"));
        levels.Add(new Level(65, 6, 7, 25, true, "Sounds/music5"));
        levels.Add(new Level(65, 3, 7, 25, true, "Sounds/music5"));

        levels.Add(new Level(70, 10, 6, 25, true, "Sounds/music5"));
        levels.Add(new Level(70, 10, 5, 25, true, "Sounds/music5"));
        levels.Add(new Level(70, 10, 4, 25, true, "Sounds/music5"));

        levels.Add(new Level(70, 10, 7, 30, true, "Sounds/music5"));
        levels.Add(new Level(70, 10, 7, 35, true, "Sounds/music5"));
        levels.Add(new Level(70, 10, 7, 40, true, "Sounds/music5"));



        currentLevelIndex = getFirstIncompleteLevel();
    }

    public Level getCurrentLevel() {
        return levels[currentLevelIndex];
    }

    public bool isLastLevel() {
        return currentLevelIndex == levels.Count - 1;
    }

    private int getFirstIncompleteLevel() {
        return LevelProgress.progress.getFirstIncompleteLevel(levels.Count);
    }
}
