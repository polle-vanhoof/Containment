using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

    public NavigationScript nav;

    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            nav.GoToMainMenu();
        }
    }

}
