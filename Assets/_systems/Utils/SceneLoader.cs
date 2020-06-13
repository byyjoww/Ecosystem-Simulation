using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public SceneReference scene;

    public void LoadSceneByReference()
    {
        AsyncLoader.LoadSceneAsync(scene, 3);
    }
}
