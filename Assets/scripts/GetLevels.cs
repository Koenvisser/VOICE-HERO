using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GetLevels : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject Scrollview1;
    public GameObject Scrollview2;
    public GameObject Scrollview3;
    public GameObject Content1;
    public GameObject Content2;
    public GameObject Content3;
    public GameObject LevelInfo;
    public GameObject MainMenu;
    public GameObject GetLevelsMenu;
    public GameObject DownloadButton;
    public GameObject PreviewContent;
    public GameObject PreviewYellowPrefab;
    public GameObject PreviewRedPrefab;
    public GameObject PreviewBluePrefab;
    public AudioSource song;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LevelsGet());
    }
    private string[] results;
    private IEnumerator LevelsGet()
    {
        string GetLevelsURL = "https://www.cdprojektblue.com/levels/getlevels.php";
        UnityWebRequest lvl_get = UnityWebRequest.Get(GetLevelsURL);
        yield return lvl_get.SendWebRequest();
        if (lvl_get.isNetworkError || lvl_get.isHttpError)
        {
            Debug.Log(lvl_get.error);
        }
        else
        {
            // Show results as text
            string result = lvl_get.downloadHandler.text;
            results = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            PlaceLevels(false);
        }
    }

    private bool isactiveprev = true;
    private void PlaceLevels(bool isactive)
    {
        if (isactive == isactiveprev)
        {
            return;
        }
        if (isactive == true)
        {
            Scrollview1.SetActive(false);
            Scrollview2.SetActive(false);
            Scrollview3.SetActive(true);
        }
        else
        {
            Scrollview1.SetActive(true);
            Scrollview2.SetActive(true);
            Scrollview3.SetActive(false);
        }
        DestroyLevels();
        int posy = 0;
        GameObject lvl_text;
        int i = 0;
        foreach (string s in results)
        {
            if (i % 2 == 0 && isactive == false)
            {
                lvl_text = Instantiate(ButtonPrefab, new Vector3(Content1.transform.position.x, Content1.transform.position.y, Content1.transform.position.z), Quaternion.identity, Content1.transform);
            }
            else if (isactive == false)
            {
                lvl_text = Instantiate(ButtonPrefab, new Vector3(Content2.transform.position.x, Content2.transform.position.y, Content2.transform.position.z), Quaternion.identity, Content2.transform);
            }
            else
            {
                lvl_text = Instantiate(ButtonPrefab, new Vector3(Content3.transform.position.x, Content3.transform.position.y, Content3.transform.position.z), Quaternion.identity, Content3.transform);
            }
            RectTransform lvl_text_rect = lvl_text.GetComponent<RectTransform>();
            lvl_text_rect.offsetMax = new Vector2(-10, posy);
            lvl_text_rect.offsetMin = new Vector2(0, Content1.GetComponent<RectTransform>().rect.height + posy - 60);
            lvl_text.GetComponentInChildren<TextMeshProUGUI>().SetText(s);
            lvl_text.GetComponent<Button>().onClick.AddListener(() => DisplayInfo(s));
            lvl_text.tag = "GetLevel";
            if (i % 2 != 0 || isactive == true)
            {
                posy -= 70;
            }
            i++;
        }
        isactiveprev = isactive;
    }

    private void DestroyLevels()
    {
        GameObject[] lvl_destroy;
        lvl_destroy = GameObject.FindGameObjectsWithTag("GetLevel");
        foreach (GameObject lvl in lvl_destroy)
        {
            Destroy(lvl);
        }
    }
    public void Back()
    {
        if (LevelInfo.activeSelf)
        {
            LevelInfo.SetActive(false);
            PlaceLevels(false);
        }
        else
        {
            MainMenu.SetActive(true);
            GetLevelsMenu.SetActive(false);
        }
    }


    private bool executed = false;
    private void DisplayInfo(string Level)
    {
        PlaceLevels(true);
        if (!LevelInfo.activeSelf)
        {
            LevelInfo.SetActive(true);
        }
        LevelInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(Level);
        if (executed == false)
        {
            DownloadButton.GetComponent<Button>().onClick.AddListener(() => Download(Level));
            executed = true;
        }
        else
        {
            DownloadButton.GetComponent<Button>().onClick.RemoveAllListeners();
            DownloadButton.GetComponent<Button>().onClick.AddListener(() => Download(Level));
        }
        StartCoroutine(LoadAudio(Level));
    }

    private void Download(string Level)
    {
        if (!Directory.Exists(Application.dataPath + "Levels/" + Level))
        {
            Directory.CreateDirectory(Application.dataPath + "Levels/" + Level);
        }
        StartCoroutine(DownloadFile(Level, "level.txt"));
        StartCoroutine(DownloadFile(Level, "song.wav"));
    }

    private IEnumerator DownloadFile(string Level, string File)
    {
        DownloadButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Downloading...");
        var uwr = new UnityWebRequest("https://www.cdprojektblue.com/levels/files/" + Level + "/" + File, UnityWebRequest.kHttpVerbGET);
        string path = Application.dataPath + "/Levels/" + Level + "/" + File;
        var dh = new DownloadHandlerFile(path)
        {
            removeFileOnAbort = true
        };
        uwr.downloadHandler = dh;
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
            Debug.Log(File + " was successfully downloaded and saved to " + path);
        DownloadButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Downloaded");
    }

    public void GoToLevelEditor()
    {
        SceneManager.LoadScene("Leveleditor");
    }
    private int levellength = 0;
    private IEnumerator LoadpreviewText(string Level)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://www.cdprojektblue.com/levels/files/" + Level + "/level.txt"))
        {
            yield return webRequest.SendWebRequest();
            int linevalue, left = 0, top;
            string line;
            GameObject CurrentColor = PreviewBluePrefab;
            GameObject Previewcircle;
            RectTransform rect;
            string longStringFromFile = webRequest.downloadHandler.text;
            List<string> lines = new List<string>(
            longStringFromFile.Split(new string[] { "\r", "\n" },StringSplitOptions.RemoveEmptyEntries));
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                line = lines[i];
                if (i == lines.Count - 1)
                {
                    int.TryParse(line, out linevalue);
                    RectTransform rt = PreviewContent.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, 80 * (linevalue - 8));
                    levellength = linevalue - 8;
                }
                else if (line == "end")
                {
                    CurrentColor = PreviewBluePrefab;
                    left = 40;
                }
                else if (line == "b")
                {
                    CurrentColor = PreviewYellowPrefab;
                    left = -40;
                }
                else if (line == "y")
                {
                    CurrentColor = PreviewRedPrefab;
                    left = 0;
                }
                else if (line != "r")
                {
                    int.TryParse(line, out linevalue);
                    Previewcircle = Instantiate(CurrentColor, new Vector3(PreviewContent.transform.position.x, PreviewContent.transform.position.y, PreviewContent.transform.position.z), Quaternion.identity, PreviewContent.transform);
                    rect = Previewcircle.GetComponent<RectTransform>();
                    top = (linevalue - 8) * -80 + 40;
                    rect.anchoredPosition = new Vector2(left,top);
                }

            }
            PreviewContent.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadAudio(string Level)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://www.cdprojektblue.com/levels/files/" + Level + "/song.wav", AudioType.WAV))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                song.clip = DownloadHandlerAudioClip.GetContent(www);
                StartCoroutine(LoadpreviewText(Level));
                song.Play();
            }
        }
    }
    private float timer;
    private void Update()
    {
        if (song.isPlaying && levellength != 0)
        {
            timer += Time.deltaTime;
            float progress = timer / (levellength * 0.1f);
            LevelInfo.transform.GetChild(1).GetComponent<Slider>().value = progress;
            PreviewContent.transform.parent.parent.GetChild(0).gameObject.GetComponent<Scrollbar>().value = 1 - progress;
            if (progress >= 1)
            {
                song.Stop();
            }
        }
    }
}
