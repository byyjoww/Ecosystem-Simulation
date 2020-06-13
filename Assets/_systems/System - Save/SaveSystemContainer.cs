using UnityEngine;

public class SaveSystemContainer : MonoBehaviour
{
    [SerializeField] SaveSystem saveSystem;

    void Awake()
    {
        DontDestroyOnLoad(this);
#if UNITY_EDITOR
        saveSystem.Init();
    }

    void OnDestroy()
    {
        saveSystem.End();
#endif
        
    }
}
