using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneInitializer : Initializer
{
    public event Action OnSceneLoadComplete;
    IEnumerator Start()
    {
        foreach (var obj in initializebles)
        {
            var initializable = (obj as IInitializable);

            initializable?.Init();
        }

        foreach (var obj in initializebles)
        {
            var initializable = (obj as IInitializable);

            if(initializable == null)
            {
                continue;
            }

            yield return new WaitUntil(() => initializable.Initialized);
        }

        OnSceneLoadComplete?.Invoke();
        yield return null;
    }
    
    public event Action<Scene> OnSceneLoad;

    // Called First
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called Second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded called: {scene}");
        OnSceneLoad?.Invoke(scene);
    }

    // Called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}