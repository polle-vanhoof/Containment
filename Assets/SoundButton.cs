using UnityEngine;

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
    }

    public void Unmute()
    {
        PlayerPrefs.SetInt("mute", 0);
        AudioListener.volume = 1;
        AudioListener.pause = false;
        PlayerPrefs.Save();
    }

	public void MuteUnmute()
    {
        if (AudioListener.volume == 1)
            Mute();
        else
            Unmute();
    }

}
