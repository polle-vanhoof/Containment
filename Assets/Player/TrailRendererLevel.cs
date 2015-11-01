using UnityEngine;
using System.Collections;

public class TrailRendererLevel : MonoBehaviour {

    private TrailRenderer trail;

	// Use this for initialization
	void Start () {
        trail = GetComponent<TrailRenderer>();
        trail.sortingLayerName = "Default";
        trail.sortingOrder = -1;
        trail.time = 9999;
        stopTrail();
    }

    public void stopTrail() {
        trail.enabled = false;
    }

    public void startTrail() {
        clearTrail();
        trail.enabled = true;
    }

    public void clearTrail() {
        trail.time = -1;
        Invoke("resetTrailTime", 0.05f); // setting to low will cause problems!
    }

    private void resetTrailTime() {
        trail.time = 9999; // no fading of the trail
    }

}
