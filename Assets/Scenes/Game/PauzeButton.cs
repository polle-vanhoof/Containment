using UnityEngine;
using UnityEngine.UI;

public class PauzeButton : MonoBehaviour {

    public GameObject score, moves;
    public Text buttonText;
    public Button replayButton, levelSelectButton, soundButton;

    void Awake() {
        Time.timeScale = 1;
        Hide(replayButton);
        Hide(levelSelectButton);
        Hide(soundButton);

    }

    public void PauzePlay() {
        if (Time.timeScale == 0) {
            play();
        } else {
            pause();
        }
    }

    private void play() {
        Time.timeScale = 1;
        Hide(replayButton);
        Hide(levelSelectButton);
        Hide(soundButton);
        score.SetActive(true);
        moves.SetActive(true);

        // change button image
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load("pause", typeof(Sprite)) as Sprite;
    }

    private void pause() {
        Time.timeScale = 0;
        Unhide(replayButton);
        Unhide(levelSelectButton);
        Unhide(soundButton);
        score.SetActive(false);
        moves.SetActive(false);

        // change button image
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load("play2", typeof(Sprite)) as Sprite;
    }

    public void pauseNoMenu() {
        Time.timeScale = 0;
    }



    public void Hide(Button but) {
        but.enabled = false;
        but.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
        Text text = but.GetComponentInChildren<Text>();
        if (text != null) {
            text.color = Color.clear;
        }
    }

    public void Unhide(Button but) {
        but.enabled = true;
        but.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
        Text text = but.GetComponentInChildren<Text>();
        if (text != null) {
            text.color = Color.black;
        }
    }

}
