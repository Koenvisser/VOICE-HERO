using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelSelectScript : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject MainCamera;
    public GameObject Content;
    bool Executed = false;
    // This script will instantiate the Prefab when the game starts and add text and an onclick event
    public void AddText()
    {
        //only execute once
        if (Executed == false)
        {
            float posy = 0;
            List<string> levelnames = new List<string>();
            string[] directories = Directory.GetDirectories(Application.dataPath + "/Resources/levels");
            foreach (string dir in directories)
            {
                if(File.Exists(dir + "/level.txt") && File.Exists(dir + "/song.wav") && !dir.Contains("LevelEditor"))
                {
                    
                    //get name of level
                    string dir2 = dir.Replace(Application.dataPath + "/Resources/levels\\", "");
                    levelnames.Add(dir2);                    
                }
            }
            posy = -24 * levelnames.Count;
            foreach (string levelname in levelnames)
            {

                //create button
                GameObject button = Instantiate(myPrefab, new Vector3(Content.transform.position.x + Content.GetComponent<RectTransform>().rect.width * 0.5f, Content.transform.position.y - Content.GetComponent<RectTransform>().rect.height * 0.5f + posy, Content.transform.position.z), Quaternion.identity, Content.transform);
                Debug.Log(Content.transform.position.x);
                Debug.Log(Content.transform.position.y);
                //set the text of the button to the name of the level
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(levelname);

                //add an onclick event to the button to play the level when the button is clicked
                button.GetComponent<Button>().onClick.AddListener(() => PlayLevel(levelname));

                //create eventtriggers for pointerenter and pointerexit for the button to display the highscores for the level when hovering over the button, 
                //and displaying the main highscores when not hovering over the button anymore
                EventTrigger.Entry eventtype = new EventTrigger.Entry();
                eventtype.eventID = EventTriggerType.PointerEnter;
                eventtype.callback.AddListener((eventData) => { if (MainCamera.activeSelf) { MainCamera.GetComponent<ScoreSetGet>().GetHighscoresbylevel(levelname); } });
                EventTrigger.Entry eventtype2 = new EventTrigger.Entry();
                eventtype2.eventID = EventTriggerType.PointerExit;
                eventtype2.callback.AddListener((eventData) => { if (MainCamera.activeSelf) { MainCamera.GetComponent<ScoreSetGet>().GetHighscores(); } });

                //add the eventtriggers to the button
                button.AddComponent<EventTrigger>();
                button.GetComponent<EventTrigger>().triggers.Add(eventtype);
                button.GetComponent<EventTrigger>().triggers.Add(eventtype2);

                //lower the position y value so the next button will be under this button
                posy -= button.GetComponent<RectTransform>().rect.height;
            }
            Executed = true;
            Content.GetComponent<RectTransform>().offsetMin = new Vector2(Content.GetComponent<RectTransform>().offsetMin.x, Content.GetComponent<RectTransform>().offsetMin.y - 50 * (levelnames.Count -  3));
        }
    }
    public void PlayLevel(string level)
    {
        //Sets the currentlevel value in the playerprefs file to the current level in order to get this value in the game
        PlayerPrefs.SetString("currentLevel", level);
        //loads the main game
        SceneManager.LoadScene("Level 1");
    }
    

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
