using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;
using System.IO;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    // Is used to manage the game
    // variables are to see if the game is paused or not
    // references to the audioplayer and pausemenu UI
    public bool GameIsPaused = false;
    public AudioSource song;
    public GameObject pauseMenu;
    public GameObject EndScreen;

    public GameObject redBeat;
    public GameObject blueBeat;
    public GameObject yellowBeat;
    public GameObject SongEnd;

    public GameObject GameScore;
    public GameObject EndScore;
    public GameObject ScoreName;
    public GameObject Video;

    public List<string> levelStringList = new List<string>();
    private string levelname = "";

    private void Start()
    {
        //load the currentlevel value set in the menu scene
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            levelname = PlayerPrefs.GetString("currentLevel");
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        if (levelname == "LevelEditor")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 1"));
        }
        //get the folder location of the level that has been selected, which must contain a level.txt file and a song.wav file
        string foldername = Application.dataPath + "/Resources/Levels/" + levelname;
        Readfile(foldername + "/level.txt");
        MakeLevel();
        string songlocation = foldername + "/song.wav";
        StartCoroutine(LoadAudio(songlocation, song));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                StopGame();
            }
        }
    }

    //load the audio using unitywebrequest to stream the audio file
    IEnumerator LoadAudio(string songlocation, AudioSource audiosource)
    {
        if (File.Exists(songlocation))
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + songlocation, AudioType.WAV))
            {
                //wait for the audio file to be ready
                yield return www.SendWebRequest();

                //check if an error has occurred
                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    audiosource.clip = DownloadHandlerAudioClip.GetContent(www);
                }
            }
            audiosource.Play();
        }
    }

    // If the game is paused but we want to start playing again we deactivate the pause UI start the time and unpause the game
    // The bool is also changed to indacate we are not paused anymore
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        song.UnPause();
        GameIsPaused = false;
        Video.GetComponent<VideoPlayer>().Play();
    }

    // If we are playing but want to pause we activate the pause menu UI, set the game time still and pause the song
    // The bool is also set to indacate the game is paused
    void StopGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        song.Pause();
        GameIsPaused = true;
        Video.GetComponent<VideoPlayer>().Pause();
    }

    // When we go to the main menu from the pause scene we first need to reset the gametime, indacate the game is not paused and stop the song.
    public void MainMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
        song.Stop();
        SceneManager.LoadScene("Menu");
    }

    // Here we read the level file and make it into a list of strings
    private void Readfile(string filename)
    {
        StreamReader reader = new StreamReader(filename);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            levelStringList.Add(line);
        }
        reader.Close();
    }

    // Here we use the list of strings to make the level
    // We do this by checking if the line had a number or a sign if it is a number we use that as a coörtdinate to instantiate the right object. A letter or word indacates we need to switch to instantiating another object.
    void MakeLevel()
    {
        float pos = 0;
        string Beat = "start";
        bool Ispos;
        for (int i = 0; i < levelStringList.Count; i++)
        {
            string line = levelStringList[i];
            try
            {
                pos = float.Parse(line);
                Ispos = true;
            }
            catch (FormatException)
            {
                Beat = line;
                Ispos = false;
            }
            if (Beat != "r" && Beat != "y" && Beat != "b" && Beat == "End")
            {

            }
            else if (Beat == "r" && Ispos)
            {
                Instantiate(redBeat, new Vector3(0, 0.25f, pos), Quaternion.identity);
            }
            else if (Beat == "y" && Ispos)
            {
                Instantiate(yellowBeat, new Vector3(-2, 0.25f, pos), Quaternion.identity);
            }
            else if (Beat == "b" && Ispos)
            {
                Instantiate(blueBeat, new Vector3(2, 0.25f, pos), Quaternion.identity);
            }
            else if (Beat == "end" && Ispos)
            {
                Instantiate(SongEnd, new Vector3(0, 0.5f, pos), Quaternion.identity);
            }
        }
    }

    // When the endsong objects collides with the playerobjects the level is over and an endscreen pops up
    public void EndGame()
    {
        if (levelname == "LevelEditor")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Leveleditor"));
        }
        else
        {
            EndScreen.SetActive(true);
            Time.timeScale = 0;
            song.Pause();
            EndScore.GetComponent<TextMeshProUGUI>().text = GameScore.GetComponent<Text>().text.Replace("Score ", "");
            if (PlayerPrefs.HasKey("ScoreName"))
            {
                ScoreName.GetComponent<TMP_InputField>().text = PlayerPrefs.GetString("ScoreName");
            }
        }
    }

    // Restarts the level
    public void Restart()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
        song.Stop();
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("level 1");
    }
}
