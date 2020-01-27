using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceEngine : MonoBehaviour
{
    string str;
    /*
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    */
    void Start()
    {
        /*actions.Add("yellow", Yellow);
        actions.Add("red", Red);
        actions.Add("blue", Blue);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Recognized;
        keywordRecognizer.Start();*/
        if (UnitySphinx.IsInitialized())
            UnitySphinx.Stop();
        UnitySphinx.Init();
        UnitySphinx.Run();
    }

    private void Update()
    {
        str = UnitySphinx.DequeueString();
        if (UnitySphinx.GetSearchModel() == "kws")
        {
            //print("listening for keyword");
            //if (str != "")
           // {
                UnitySphinx.SetSearchModel(UnitySphinx.SearchModel.jsgf);
                print(str);
            //}
        }
        else if (UnitySphinx.GetSearchModel() == "jsgf")
        {
            print("listing for colours");
            if (str!="")
            {
                print(str);
                if (str == "yellow")
                    Yellow();
                else if (str == "red")
                    Red();
                else if (str == "blue")
                    Blue();
                else if (str == "green")
                {
                    Yellow();
                    Blue();
                }
                else if (str == "purple")
                {
                    Red();
                    Blue();
                }
                else if (str == "orange")
                {
                    Yellow();
                    Red();
                }
                else if (str == "brown")
                {
                    Yellow();
                    Red();
                    Blue();
                }
            }
        }
    }
    /*
    private void Recognized(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }*/

    private void Yellow()
    {
        PlayerActionYellow.RecognizedSpeech("yellow");
    }

    private void Red()
    {
        PlayerActionRed.RecognizedSpeech("red");
    }

    private void Blue()
    {
        PlayerActionBlue.RecognizedSpeech("blue");
    }
}
