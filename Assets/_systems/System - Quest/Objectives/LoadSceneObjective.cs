using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneObjective : QuestObjective
{
    [SerializeField] private SceneReference scene;

    #region ON_SCENE_LOAD
    public void OnEnable()
    {
        // Called First
        // Debug.Log("OnSceneLoaded subscribed to scene load.");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.path == this.scene)
        {
            CompleteObjective();
        }
        else
        {
            IncompleteObjective();
        }
    }

    public override void CheckObjective()
    {
        if (SceneManager.GetActiveScene().path == scene)
        {
            CompleteObjective();
        }
    }
    
    public void OnDisable()
    {
        // Called Last
        // Debug.Log("OnSceneLoaded unsubscribed to scene load.");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion
}