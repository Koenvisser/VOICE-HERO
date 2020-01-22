using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelEditor : MonoBehaviour
{
    public GameObject YellowPrefab;
    public GameObject RedPrefab;
    public GameObject BluePrefab;
    public GameObject Viewport;
    public GameObject Content;
    public GameObject AudioBar;
    public GameObject AudioLine;
    public GameObject SongInputField;
    public GameObject NameInputField;
    public GameObject scrollbar;
    public GameObject YellowButton;
    public GameObject RedButton;
    public GameObject BlueButton;
    public AudioSource song;
    private string songlocation = "";
    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator ImportAudio(string songlocation, AudioSource audiosource) {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(songlocation, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                audiosource.clip = DownloadHandlerAudioClip.GetContent(www);
                float songlength = audiosource.clip.length;
                SetWidth(songlength);
            }
        }
    }

    private void SetWidth(float songlength) {
        RectTransform rt = Content.GetComponent<RectTransform>();
        rt.offsetMax = new Vector2(songlength * 120 - 648, rt.offsetMax.y);
    }

    public void OnYellowClick()
    {
        OnContentClick(1);

    }

    public void OnRedClick()
    {
        OnContentClick(2);

    }

    public void OnBlueClick()
    {
        OnContentClick(3);
    }
    private List<int> yellowlist = new List<int>();
    private List<int> redlist = new List<int>();
    private List<int> bluelist = new List<int>();
    private void OnContentClick(int colour)
    {
        float mousepos = (Content.GetComponent<RectTransform>().rect.size.x - Viewport.GetComponent<RectTransform>().rect.size.x) * scrollbar.GetComponent<Scrollbar>().value + 1.15f * (Input.mousePosition.x - 40);
        int left = (int)mousepos / 120;
        Debug.Log(left + 8);
        mousepos = 120 * left - Viewport.transform.position.x + 66.7f;
        if (colour == 1)
        {
            GameObject Circle = Instantiate(YellowPrefab, new Vector3(0, 0, YellowButton.transform.position.z), Quaternion.identity, YellowButton.transform);
            Circle.GetComponent<RectTransform>().offsetMax = new Vector2((-(Content.GetComponent<RectTransform>().rect.size.x - mousepos - 60)), 0);
            Circle.GetComponent<RectTransform>().offsetMin = new Vector2(mousepos - 60, 0);
            yellowlist.Add(left + 8);
        }
        else if (colour == 2)
        {
            GameObject Circle = Instantiate(RedPrefab, new Vector3(0, 0, RedButton.transform.position.z), Quaternion.identity, RedButton.transform);
            Circle.GetComponent<RectTransform>().offsetMax = new Vector2((-(Content.GetComponent<RectTransform>().rect.size.x - mousepos - 60)), 0);
            Circle.GetComponent<RectTransform>().offsetMin = new Vector2(mousepos - 60, 0);
            redlist.Add(left + 8);
        }
        else if (colour == 3)
        {
            GameObject Circle = Instantiate(BluePrefab, new Vector3(0, 0, BlueButton.transform.position.z), Quaternion.identity, BlueButton.transform);
            Circle.GetComponent<RectTransform>().offsetMax = new Vector2((-(Content.GetComponent<RectTransform>().rect.size.x - mousepos - 60)), 0);
            Circle.GetComponent<RectTransform>().offsetMin = new Vector2(mousepos - 60, 0);
            bluelist.Add(left + 8);
        }
    }
    public void TestLevel() {
        CopyFiles(Application.dataPath + "/Levels/LevelEditor/");
        PlayerPrefs.SetString("currentLevel", "LevelEditor");
        SceneManager.LoadScene("Level 1");
    }

    public void Reset() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void SaveLevel() {
        string path = Application.dataPath + "/Levels/" + NameInputField.GetComponent<TMP_InputField>().text + "/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
            CopyFiles(path);
    }

    private void CopyFiles(string path) {
        string leveltext = CreateLevelText();
        StreamWriter writer = new StreamWriter(path + "level.txt", false);
        writer.WriteLine(leveltext);
        writer.Close();
        if (File.Exists(path + "song.wav"))
        {
            File.Delete(path + "song.wav");
        }
        if (songlocation != "" && File.Exists(songlocation))
        {
            File.Copy(songlocation, path + "song.wav");
        }
        }

    string CreateLevelText() {
        string leveltext = "r\n";
        redlist.Sort();
        yellowlist.Sort();
        bluelist.Sort();
      
        int[] maxvalues = { redlist[redlist.Count - 1], yellowlist[yellowlist.Count - 1], bluelist[bluelist.Count - 1]};
        
        foreach (int i in redlist)
        {
            leveltext += i + "\n";
        }
        leveltext += "y\n";
        foreach (int i in yellowlist)
        {
            leveltext += i + "\n";
        }
        leveltext += "b\n";
        foreach (int i in bluelist)
        {
            leveltext += i + "\n";
        }
        leveltext += "end\n" + (maxvalues.Max() + 2);
        return leveltext;
    }

    public void AddAudio() {
        songlocation = SongInputField.GetComponent<TMP_InputField>().text;
        StartCoroutine(ImportAudio(songlocation,song));

    }

    public void MainMenu() {
        SceneManager.LoadScene("Menu");
    }
    public void PlayAudio()
    { song.Play(); }

    public void PauseAudio()
    { song.Pause(); }

    private void Update()
    {
        if (song.isPlaying)
        {
            AudioLine.GetComponent<RectTransform>().localPosition = new Vector2(song.time * 120, AudioLine.GetComponent<RectTransform>().localPosition.y) ;
        }
    }
}
