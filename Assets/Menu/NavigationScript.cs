using UnityEngine;

public class NavigationScript : MonoBehaviour {
    
    //Main Menu

    public void GoToMainMenu()
    {
        Application.LoadLevel("Menu");
    }

    //Level Select

    public void GoToLevelSelect()
    {
        Application.LoadLevel("LevelSelect");
    }

    public void startNextLevel()
    {
        GameSetup.levelManager.currentLevelIndex++;
        ContinueGame();
    }
    
    public void startLevel(int levelIndex)
    {
        GameSetup.levelManager.currentLevelIndex = levelIndex;
        ContinueGame();
    }

    //Play / Continue

    private AsyncOperation async = null;
    public Texture2D emptyProgressBar;
    public Texture2D fullProgressBar;

    public void ContinueGame()
    {
        async = Application.LoadLevelAsync("Containment");
    }

    void OnGUI()
    {
        if (async != null)
        {
            GUI.DrawTexture(new Rect(0, 0, 100, 50), emptyProgressBar);
            GUI.DrawTexture(new Rect(0, 0, 100 * async.progress, 50), fullProgressBar);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(0, 0, 100, 50), string.Format("{0:N0}%", async.progress * 100f));
        }
    }

}
