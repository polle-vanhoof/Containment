using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

public class LevelSelectPopulator : MonoBehaviour {

    public LevelManager levelManager;
    public GameObject levelCanvas;
    public GameObject levelPickObject;
    public NavigationScript navigation;

    private int page = 1;
    private LinkedList<GameObject> levels = new LinkedList<GameObject>();

    void Start() {
        levelManager = new LevelManager();
        populate();
    }

    private void populate() {
        clearLevels();
        int pageMin = (page - 1) * 12;
        int pageMax = page * 12;
        if (pageMax > levelManager.levels.Count) {
            pageMax = levelManager.levels.Count;
        }
        for (int i = pageMin; i < pageMax; i++) {
            GameObject levelPick = (GameObject)Instantiate(levelPickObject, new Vector3(), new Quaternion());
            levelPick.transform.SetParent(levelCanvas.transform);
            levels.AddLast(levelPick);
            levelPick.GetComponent<TextMesh>().text = (i + 1) + "";

            EventTrigger et = levelPick.gameObject.GetComponent<EventTrigger>();
            if (et == null)
                et = levelPick.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            int j = i; //~CPL
            entry.callback.AddListener((eventData) => { navigation.startLevel(j); });
            et.triggers.Add(entry);
        }
    }

    private void clearLevels() {
        foreach (GameObject level in levels) {
            level.transform.SetParent(null);
            level.GetComponent<TextMesh>().text = "";
            Destroy(level);
        }
        levels.Clear();
    }

    public void nextPage() {
        if (page * 12 < levelManager.levels.Count) {
            page++;
            populate();
        }
    }

    public void prevPage() {
        if (page > 1) {
            page--;
            populate();
        }
    }
}
