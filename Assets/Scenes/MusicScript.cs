using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    public static MusicScript music;
    private bool inLevel = false;
    private string currentSong = "";

    void Awake() {
        if (music == null) {
            DontDestroyOnLoad(gameObject);
            music = this;
        } else if (music != this) {
            Destroy(gameObject);
        }
    }


    public void play(string songPath) {
        if (currentSong.Equals(songPath)) {
            return;
        }
        currentSong = songPath;
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = (AudioClip)Resources.Load(songPath, typeof(AudioClip));
        audio.Play();
        audio.loop = true;
    }

    public string getCurrentSong() {
        return currentSong;
    }
}
