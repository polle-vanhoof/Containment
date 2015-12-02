using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MuteButtonMain : MonoBehaviour {

    void Start() {
        if (PlayerPrefs.GetInt("mute") == 1)
            Mute();
        else
            Unmute();
    }

    public void Mute() {
        PlayerPrefs.SetInt("mute", 1);
        AudioListener.volume = 0;
        AudioListener.pause = true;
        PlayerPrefs.Save();

        // change button image
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load("volume_mute", typeof(Sprite)) as Sprite;
    }

    public void Unmute() {
        PlayerPrefs.SetInt("mute", 0);
        AudioListener.volume = 1;
        AudioListener.pause = false;
        PlayerPrefs.Save();

        // change button image
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load("volume", typeof(Sprite)) as Sprite;
    }

    public void MuteUnmute() {
        if (AudioListener.volume == 1)
            Mute();
        else
            Unmute();
    }

}
