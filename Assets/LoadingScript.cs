using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{

    public void LoadLevel()
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        //loads the main game
        AsyncOperation Loading = SceneManager.LoadSceneAsync("Level 1");

        while (!Loading.isDone)
        {
            float progress = Mathf.Clamp01(Loading.progress / .9f);
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = progress;
            yield return 0;
        }
    }
}
