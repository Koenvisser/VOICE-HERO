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
    public GameObject Scrollview;
    public GameObject Content;
    public GameObject LevelInfo;
    public GameObject MainMenu;
    public GameObject GetLevelsMenu;
    public GameObject DownloadButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelsGet());
    }

    private IEnumerator LevelsGet() {
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
            string[] results = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int posy = 0;
            GameObject lvl_text;
            foreach (string s in results)
            {
                    lvl_text = Instantiate(ButtonPrefab, new Vector3(Content.transform.position.x, Content.transform.position.y, Content.transform.position.z), Quaternion.identity, Content.transform);
                RectTransform lvl_text_rect = lvl_text.GetComponent<RectTransform>();
                lvl_text_rect.offsetMax = new Vector2(-10, posy);
                lvl_text_rect.offsetMin = new Vector2(0, Content.GetComponent<RectTransform>().rect.height + posy - 60);
                lvl_text.GetComponentInChildren<TextMeshProUGUI>().SetText(s);
                lvl_text.GetComponent<Button>().onClick.AddListener(() => DisplayInfo(s));
                posy -= 70;
            }
            }
        }

    public void Back()
    {
        if (LevelInfo.activeSelf)
        {
            LevelInfo.SetActive(false);
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
        if (!LevelInfo.activeSelf)
        {
            LevelInfo.SetActive(true);
        }
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
    }

    private void Download(string Level) {
        if (!Directory.Exists(Application.dataPath + "Levels/" + Level))
        {
            Directory.CreateDirectory(Application.dataPath + "Levels/" + Level);
        }
        StartCoroutine(DownloadFile(Level, "level.txt"));
        StartCoroutine(DownloadFile(Level, "song.wav"));
    }

    private IEnumerator DownloadFile(string Level, string File)
    {
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
            Debug.Log(File + "was successfully downloaded and saved to " + path);
    }

    public void GoToLevelEditor() {
        SceneManager.LoadScene("Leveleditor");
    }
}
