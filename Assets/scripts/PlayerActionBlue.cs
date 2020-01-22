using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBlue : MonoBehaviour
{
    // conections to the transform and Beatcollision components of the playerbutton
    public Transform scale;
    public BeatCollision bc;
    // easy to change variables for colour of player and key to press
    public string Colour;
    public string Key;
    // start of the voice recognision sofware
    bool blue = false;
    public GameObject Fire;
    public static PlayerAction playeraction;

    // checks if the button press is at the right time to give points or if not to end the streak
    private void Rainbow()
    {
        Pressed();
        if (bc.Presswindow == true)
        {
            GameObject.Destroy(bc.CurrentBeat);
            FindObjectOfType<Score>().score += 200;
            FindObjectOfType<Streak>().streak += 1;
            FirePress();
            bc.Presswindow = false;
        }
        else
        {
            FindObjectOfType<Streak>().streak = 0;
        }
    }

    public static void RecognizedSpeech(string color)
    {
        if (color == "blue")
        {
            FindObjectOfType<PlayerActionBlue>().blue = true;
        }
    }


    // checks if the button press is at the right time to give points or if not to end the streak
    void Update()
    {
        if (blue)
        {
            blue = false;
            Pressed();
            if (bc.Presswindow == true)
            {
                GameObject.Destroy(bc.CurrentBeat);
                FindObjectOfType<Score>().score += 200;
                FindObjectOfType<Streak>().streak += 1;
                FirePress();
                bc.Presswindow = false;
            }
            else
            {
                FindObjectOfType<Streak>().streak = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<GameManager>().Pause();
        }
    }

    // pushes the button down on press
    void Pressed()
    {
        scale.localScale = new Vector3(1, 0.2f, 1);
        scale.localPosition -= new Vector3(0, 0.05f, 0);
        Invoke("ButtonUp", 0.1f);
    }

    // brings the button back to normal
    void ButtonUp()
    {
        scale.localScale = new Vector3(1, 0.25f, 1);
        scale.localPosition += new Vector3(0, 0.05f, 0);
    }

    // Here a fire is instantiated on the player button if a correct hit is scored the fires destroy themselfs
    void FirePress()
    {
        if (Colour == "red")
        {
            Instantiate(Fire, new Vector3(0, 0.4f, 7.5f), Quaternion.Euler(90, 0, 0));
        }
        else if (Colour == "yellow")
        {
            Instantiate(Fire, new Vector3(-2, 0.4f, 7.5f), Quaternion.Euler(90, 0, 0));
        }
        else if (Colour == "blue")
        {
            Instantiate(Fire, new Vector3(2, 0.4f, 7.5f), Quaternion.Euler(90, 0, 0));
        }
    }
}
