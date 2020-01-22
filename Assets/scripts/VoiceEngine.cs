using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceEngine : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start()
    {
        actions.Add("yellow", Yellow);
        actions.Add("red", Red);
        actions.Add("blue", Blue);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += Recognized;
        keywordRecognizer.Start();
    }

    private void Recognized(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

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
