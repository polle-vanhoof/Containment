using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LevelSelectPopulator : MonoBehaviour {

    public LevelManager levelManager;
    public GameObject levelCanvas;
    public GameObject levelPickObject;
    public NavigationScript navigation;
        
	void Start () {
        levelManager = new LevelManager();
        for(int i = 0; i < levelManager.levels.Count; i++)
        {
            GameObject levelPick = (GameObject)Instantiate(levelPickObject, new Vector3(), new Quaternion());
            levelPick.transform.SetParent(levelCanvas.transform);

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
}
