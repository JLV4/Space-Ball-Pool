using UnityEngine;

public class MenuScripts : MonoBehaviour
{
    public GameSettings gameSettings;
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        gameSettings.gameOver = false;
        gameSettings.p1Score = 0;
        gameSettings.p2Score = 0;
        gameSettings.currPlayer = 1;
        gameSettings.p1Stripes = null;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    public void LoadSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }
    
    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
