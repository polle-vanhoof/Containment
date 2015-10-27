using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {

    void OnGUI() {
        if (GameSetup.debugMode) {
            foreach (Touch touch in Input.touches) {
                string message = "";
                message += "ID: " + touch.fingerId + "\n";
                message += "Phase: " + touch.phase.ToString() + "\n";
                message += "TapCount: " + touch.tapCount + "\n";
                message += "Pos x: " + touch.position.x + "\n";
                message += "Pos y: " + touch.position.y + "\n";

                int num = touch.fingerId;
                GUI.Label(new Rect(0 + 130 * num, 150, 120, 100), message);
            }
        }
    }
}
