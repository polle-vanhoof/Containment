using UnityEngine;
using System.Collections;

public class MusicSetup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MusicScript.music.play("Sounds/MenuMusic");
	}
}
