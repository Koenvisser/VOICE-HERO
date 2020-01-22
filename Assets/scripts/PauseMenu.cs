using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Is used when the main menu button is pressed in the pause screen
    public void Menu()
    {
        FindObjectOfType<GameManager>().MainMenu();
    }
    //  Is used for a restart button
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Is called when the resume button is pressed in the pause menu and resumes the game.
    public void ResumeGame()
    {
        FindObjectOfType<GameManager>().Resume();
    }
}
