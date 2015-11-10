using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {
    
	public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
