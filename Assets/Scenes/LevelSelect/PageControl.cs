using UnityEngine;
using System.Collections;

public class PageControl : MonoBehaviour {


    public LevelSelectPopulator populator;

    public void nextPage() {
        populator.nextPage();
    }

    public void prevPage() {
        populator.prevPage();
    }
}
