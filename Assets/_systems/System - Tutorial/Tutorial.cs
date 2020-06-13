using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public SavableIntValue startTutorial;
    public GameObject tutorialPopup;

    [Header("Sprites")]
    public Sprite placeholderSprite;

    private void Start()
    {        
        if (startTutorial.Value == 1)
        {
            OnChangedToMainMenu += StartTutorial;
        }
    }

    public void StartTutorial()
    {
        startTutorial.Value = 0;
        OnChangedToMainMenu -= StartTutorial;
    }

    public void EndTutorial()
    {
        
    }

    //------------------END TUTORIAL------------------

    public void CreatePopup(string title, string content, Sprite sprite, Action method)
    {
        //CreateTutorialPopup(tutorialPopup, FindObjectOfType<Canvas>().transform, title, content, sprite, method);
    }

    //public GameObject CreateTutorialPopup(GameObject element, Transform parent, string title, string description, Sprite sprite, Action method)
    //{
    //    GameObject popup = GameObject.Instantiate(element, parent);
    //    popup.GetComponent<SetupTutorialPopup>().Setup(title, description, sprite, method);
    //    return popup;
    //}

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu")
        {
            OnChangedToMainMenu?.Invoke();
        }

        if(scene.name == "InGameEasy" || scene.name == "InGameMedium" || scene.name == "InGameHard")
        {
            OnChangedToInGame?.Invoke();
        }
    }

    public event Action OnChangedToMainMenu;
    public event Action OnChangedToInGame;

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}