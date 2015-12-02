using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

    void Start()
    {
        if (PlayerPrefs.GetInt("mute") == 1)
            Mute();
        else
            Unmute();
    }

    public void Mute()
    {
        PlayerPrefs.SetInt("mute", 1);
        AudioListener.volume = 0;
        AudioListener.pause = true;
        PlayerPrefs.Save();

        // change button image
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load("unmute", typeof(Sprite)) as Sprite;
    }

    public void Unmute()
    {
        PlayerPrefs.SetInt("mute", 0);
        AudioListener.volume = 1;
        AudioListener.pause = false;
        PlayerPrefs.Save();

        // change button image
        Image image = GetComponent<Image>();
        image.sprite = Resources.Load("mute", typeof(Sprite)) as Sprite;
    }

	public void MuteUnmute()
    {
        if (AudioListener.volume == 1)
            Mute();
        else
            Unmute();
    }

}
