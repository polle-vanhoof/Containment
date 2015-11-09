using UnityEngine;
using System.Collections;

public class LevelSelectButton : MonoBehaviour {
    
	public void GoToLevelSelect()
    {
        Application.LoadLevel("menu");
    }
	
}
