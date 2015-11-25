using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class LevelProgress : MonoBehaviour {


    public static LevelProgress progress;

    private LinkedList<int> completedLevels = new LinkedList<int>();

    private LinkedList<int> unlockedLevels = new LinkedList<int>();


    void Awake() {
        if (progress == null) {
            DontDestroyOnLoad(gameObject);
            progress = this;
            load();
        } else if (progress != this) {
            Destroy(gameObject);
        }
    }

    public bool isLevelCompleted(int levelNumber) {
        if (completedLevels.Contains(levelNumber)) {
            return true;
        }
        return false;
    }

    public bool isLevelUnlocked(int levelNumber) {
        if (unlockedLevels.Contains(levelNumber)) {
            return true;
        }
        return false;
    }

    public void completeLevel(int levelNumber) {
        if (!completedLevels.Contains(levelNumber)) {
            completedLevels.AddLast(levelNumber);
            unlockedLevels.AddLast(getFirstLockedLevel());
        }
    }

    public int getFirstIncompleteLevel(int numLevels) {
        for (int i = 0; i < numLevels; i++) {
            if (!isLevelCompleted(i)) {
                return i;
            }
        }
        return numLevels - 1;
    }

    public int getFirstLockedLevel() {
        for (int i = 0; i < Int32.MaxValue; i++) {
            if (!isLevelUnlocked(i)) {
                return i;
            }
        }
        return Int32.MaxValue;
    }

    public void save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/levelData.dat");
        Debug.Log("Saved level data to: " + Application.persistentDataPath + "/levelData.dat");
        LevelData data = new LevelData();
        data.completedLevels = this.completedLevels;
        data.unlockedLevels = this.unlockedLevels;

        bf.Serialize(file, data);
        file.Close();
    }

    private void load() {
        if(File.Exists(Application.persistentDataPath + "/levelData.dat")) {
            Debug.Log("Loading from: " + Application.persistentDataPath + "/levelData.dat");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/levelData.dat", FileMode.Open);
            LevelData data = (LevelData)bf.Deserialize(file);
            file.Close();

            this.completedLevels = data.completedLevels;
            this.unlockedLevels = data.unlockedLevels;
        } else {
            unlockedLevels.AddLast(0);
            unlockedLevels.AddLast(1);
        }
    }

    [Serializable]
    class LevelData {
        public LinkedList<int> completedLevels = new LinkedList<int>();
        public LinkedList<int> unlockedLevels = new LinkedList<int>();
    }


}
