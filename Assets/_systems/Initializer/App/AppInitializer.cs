using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppInitializer : Initializer
{   
    public SceneReference mainScene;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

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

        AsyncLoader.LoadSceneAsync(mainScene, 3);

        yield return null;
    }
}
