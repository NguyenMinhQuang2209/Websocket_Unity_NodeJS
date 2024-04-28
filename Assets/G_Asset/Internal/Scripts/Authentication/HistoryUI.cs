using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HistoryUI : MonoBehaviour
{
    public Transform container;
    public Scrollbar scrollbar;
    public float scrollBarTargetValue = 1f;
    public HistoryUIItem historyUIItem;

    private string URL = "";
    private void Start()
    {
        URL = API.instance.URL;
        string username = PlayerPrefs.GetString("username");
        if (username != null)
        {
            StartCoroutine(GetHistory(username));
        }
    }
    public IEnumerator GetHistory(string username)
    {
        string getURL = URL + "/history/" + username;
        using (UnityWebRequest www = new(getURL, "GET"))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                string data = www.downloadHandler.text;
                List<HistoryData> historyDatas = JsonConvert.DeserializeObject<List<HistoryData>>(data);
                for (int i = 0; i < historyDatas.Count; i++)
                {
                    HistoryData historyData = historyDatas[i];
                    HistoryUIItem item = Instantiate(historyUIItem, container.transform);
                    item.HistoryUIItemInit(historyData.survivalTime, historyData.dateText);
                }
            }
        }
    }
}
public class HistoryData
{
    public string username;
    public string dateText;
    public string survivalTime;
    public string point;
    public HistoryData(string username, string survivalTime, string point)
    {
        this.username = username;
        this.survivalTime = survivalTime;
        this.point = point;
    }
}