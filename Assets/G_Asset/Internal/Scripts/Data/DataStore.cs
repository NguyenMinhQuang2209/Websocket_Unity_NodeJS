using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStore : MonoBehaviour
{
    public static DataStore instance;
    private Dictionary<string, string> datas = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void SetData(string key, string value)
    {
        datas[key] = value;
    }
    public string GetData(string key)
    {
        return datas[key];
    }
}
