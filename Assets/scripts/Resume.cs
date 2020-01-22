using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    // Is called when the resume button is pressed in the pause menu and resumes the game.
    public void ResumeGame()
    {
        FindObjectOfType<GameManager>().Resume();
    }
}
