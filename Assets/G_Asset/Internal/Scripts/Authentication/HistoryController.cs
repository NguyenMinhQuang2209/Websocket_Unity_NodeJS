using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HistoryController : MonoBehaviour
{
    public static HistoryController instance;
    private string URL = "";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        URL = API.instance.URL;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreateHistory("Thời gian sinh tồn 20s", "40");
        }
    }
    public void CreateHistory(string survivalTime, string point)
    {
        string username = PlayerPrefs.GetString("username");
        HistoryData historyData = new(username, survivalTime, point);
        StartCoroutine(CreateHistory(username, historyData));
    }
    public IEnumerator CreateHistory(string username, HistoryData data)
    {
        string historyURL = URL + "/history/" + username;

        string jsonData = JsonConvert.SerializeObject(data);

        using (UnityWebRequest www = new(historyURL, "POST"))
        {
            byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(rawData);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                Debug.Log("Success");
            }
        }
    }
}
