using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Screen : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("ok");
            int scene = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(scene);
        }
    }
}
