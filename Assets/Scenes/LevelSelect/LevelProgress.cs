using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class LevelProgress : MonoBehaviour {


    public static LevelProgress progress;

    private LinkedList<int> completedLevels = new LinkedList<int>();

    public int unlockedLevelIndex { set; get; }


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

    public void completeLevel(int levelNumber) {
        if (!completedLevels.Contains(levelNumber)) {
            completedLevels.AddLast(levelNumber);
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

    public void save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/levelData.dat");
        Debug.Log("Saved level data to: " + Application.persistentDataPath + "/levelData.dat");
        LevelData data = new LevelData();
        data.completedLevels = this.completedLevels;
        data.unlockedLevelIndex = this.unlockedLevelIndex;

        bf.Serialize(file, data);
        file.Close();
    }

    private void load() {
        if(File.Exists(Application.persistentDataPath + "/levelData.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/levelData.dat", FileMode.Open);
            LevelData data = (LevelData)bf.Deserialize(file);
            file.Close();

            this.completedLevels = data.completedLevels;
            this.unlockedLevelIndex = data.unlockedLevelIndex;
            /*completedLevels.AddLast(0); completedLevels.AddLast(1); completedLevels.AddLast(2);*/
        } else {
            completedLevels.AddLast(0); completedLevels.AddLast(1); completedLevels.AddLast(2);
            completedLevels.AddLast(3); completedLevels.AddLast(4); completedLevels.AddLast(5);
            /*completedLevels.AddLast(6); completedLevels.AddLast(7); completedLevels.AddLast(8);
            completedLevels.AddLast(9); completedLevels.AddLast(10); completedLevels.AddLast(11);
            completedLevels.AddLast(12);*/
            unlockedLevelIndex = 11;
        }
    }

    [Serializable]
    class LevelData {
        public LinkedList<int> completedLevels = new LinkedList<int>();
        public int unlockedLevelIndex { set; get; }
    }


}
