using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class GetLevels : MonoBehaviour
{
    public GameObject ButtonPrefab;
    public GameObject Scrollview;
    public GameObject Content;
    public GameObject LevelInfo;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LevelsGet());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            int posy = 50;
            GameObject lvl_text;
            foreach (string s in results)
            {
                    lvl_text = Instantiate(ButtonPrefab, new Vector3(Content.transform.position.x, Content.transform.position.y, Content.transform.position.z), Quaternion.identity, Content.transform);
                StartCoroutine(Position(lvl_text, posy, s));
               
                    posy -= 150;
            }
            }
        }

    private IEnumerator Position(GameObject lvl_text, int posy, string s)
    {
        yield return 0;
        RectTransform lvl_text_rect = lvl_text.GetComponent<RectTransform>();
        lvl_text_rect.offsetMax = new Vector2(0, posy);
        lvl_text_rect.offsetMin = new Vector2(0, Content.GetComponent<RectTransform>().rect.height - posy - 50);
        lvl_text.GetComponentInChildren<TextMeshProUGUI>().SetText(s);
    }
    public void GoToLevelEditor() {
        SceneManager.LoadScene("Leveleditor");
    }
}
