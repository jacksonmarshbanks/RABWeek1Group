using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public void OnClickQuitButton()
    {
        print("Quit button was clicked");
        Application.Quit();
    }

    public void RetryButton()
    {
        print("Retry button was clicked");
        SceneManager.LoadScene("LevelOne");
    }

    public void MainMenuButton()
    {
        print("Main Menu button was clicked");
        SceneManager.LoadScene("MainMenu");
    }

    public void InstructionsButton()
    {
        print("Instructions button was clicked");
        SceneManager.LoadScene("HELP");
    }
}
