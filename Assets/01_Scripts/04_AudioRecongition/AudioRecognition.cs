using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
public class AudioRecognition : MonoBehaviour
{
    
    public GameObject[] entity;
   
    public List<string> currentPhrase = new List<string>();

    public List<string> allPhrases = new List<string>();
    #region SAL

    public List<string> salPhrases = new()
    {
        "imperat tibi deus exire" , 
        "impera tibi deus exire ",
        "imerat tibi deus exire",
        "imperati tibi deus exire",
        "imperat ti deus exire",
        "imperat tib deus exire",
        "imperat tibi de deus exire",
        "imperat tibi deus exir",
        "imperat tibi deus exirer",
        "emperat tibi deus exire",
        "imperat tibbi deus exire",
        "impera tibi deus exire",
        "imperatu tibi deus exire",
        "imperat tibi deuexire",
        "impert tibi deus exire",
        "imperat tibi deuses exire",
        "imperatt tibi deus exire",
        "impert tibi deuss exire",
        "imperat tibideus exire",
        "impearat tibi deus exire",
        "imperatt tibi deus exira",
        "imperat tibi deus exrie",
    };

    #endregion

    #region ABRIR PUERTA

    public List<string> abrirPhrases = new()
    {

        "Aperire in nomine lesu",
        "Aperire in nomine Iesu",
        "Aperire in nomine Jesu",
        "Aperire in nomine Iesus",
        "Aperire en nomine Iesu",
        "Aperire in nomine Iesu",
        "Apere in nomine Iesu",
        "Aperire en nomine Iesus",
        "Aperire in nomene Iesu",
        "Aperire in nomen,e Iesus",
        "Aperir en nomine Jesu",
        "Aperir in nomine Iesus",
        "Aperire in nomine Ies�",
        "Aperire en nomine Iesu",
        "Aperir in nomene Iesus",
        "Aperire en nomene Ies�",
        "Apirire en nomine Iesu",
        "Apirire en nomine Iesus",
        "Aperire in nomene Jesu",
        "Aperire en nonime Iesu",
        "Aperire en nonime Iesus"
    };

    #endregion

    enum CurrentPhrase
    {
        AbrirPuerta,
        Sal
    }
    KeywordRecognizer keywordRecognizer;
    Dictionary<string[], Action> wordToAction;
    void Start()
    {
        
        wordToAction = new Dictionary<string[], Action>();

        allPhrases.AddRange(salPhrases);
        allPhrases.AddRange(abrirPhrases);
        
        wordToAction.Add(salPhrases.ToArray(), Sal);
        wordToAction.Add(abrirPhrases.ToArray(), AbrePuerta);

        keywordRecognizer = new KeywordRecognizer(allPhrases.ToArray());
        
        keywordRecognizer.OnPhraseRecognized += WordRecognized;
        keywordRecognizer.Start();
        
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void WordRecognized(PhraseRecognizedEventArgs word)
    {
        Debug.Log(word.text);
        filterWord(word.text);
        
    }
    private void filterWord(string word)
    {
        if (salPhrases.Contains(word)) { Sal(); }   
        if (abrirPhrases.Contains(word)) { AbrePuerta(); }
    }

    private void Sal()
    {
        Debug.Log("saliendo lol");
        entity[0].GetComponent<EntityLogic>().hasTeleported = true;
        entity[0].GetComponent<EntityLogic>().lifePoints--;
    }

    private void AbrePuerta()
    {
        Application.Quit();
        Debug.Log("Abrir");
    }
}
