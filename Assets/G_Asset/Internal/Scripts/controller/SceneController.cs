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
    }
    public enum SceneName
    {
        Main,
        Auth
    }
    public void LoadNewScene(SceneName name, bool isSingle)
    {
        LoadSceneMode newMode = isSingle ? LoadSceneMode.Single : LoadSceneMode.Additive;
        SceneManager.LoadScene(name.ToString(), newMode);
    }
}
