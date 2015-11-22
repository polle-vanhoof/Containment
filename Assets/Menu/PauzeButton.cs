using UnityEngine;
using UnityEngine.UI;

public class PauzeButton : MonoBehaviour {

    public GameObject score, moves;
    public Text buttonText;
    public Button replayButton, levelSelectButton, soundButton;

    void Start() {
        Time.timeScale = 1;
        buttonText.text = "||";
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
        buttonText.text = "||";
        Hide(replayButton);
        Hide(levelSelectButton);
        Hide(soundButton);
        score.SetActive(true);
        moves.SetActive(true);
    }

    private void pause() {
        Time.timeScale = 0;
        buttonText.text = "►";
        Unhide(replayButton);
        Unhide(levelSelectButton);
        Unhide(soundButton);
        score.SetActive(false);
        moves.SetActive(false);
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
