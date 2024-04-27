using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public enum SceneName
    {
        Main_UI,
        Play,
        Auth
    }
    public void LoadNewScene(SceneName name, bool isSingle)
    {
        LoadSceneMode newMode = isSingle ? LoadSceneMode.Single : LoadSceneMode.Additive;
        SceneManager.LoadScene(name.ToString(), newMode);
    }
}
