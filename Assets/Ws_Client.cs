using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using System;
public class Ws_Client : MonoBehaviour
{

    WebSocket ws;
    string ipAddress = "ws://localhost:3000";

    public static string JOIN_PLAY = "Join_Room";

    private void Start()
    {
        ws = new WebSocket(ipAddress);
        ws.OnOpen += (sender, e) => OnWebSocketOpen(sender, e);
        ws.OnMessage += (sender, e) => OnMessageReceived(sender, e);
        ws.OnError += (sender, e) => OnWebSocketError(sender, e);
        ws.OnClose += (sender, e) => OnWebSocketClose(sender, e);

        ws.Connect();
    }
    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connected!");
    }

    void OnMessageReceived(object sender, MessageEventArgs e)
    {
        string jsonData = e.Data;
        try
        {
            DataReceive dataReceive = JsonUtility.FromJson<DataReceive>(jsonData);
            switch (dataReceive.eventName)
            {
                case "JoinRoom":
                    DataReceivePlayerData playerData = JsonConvert.DeserializeObject<DataReceivePlayerData>(jsonData);
                    if (playerData.data == null)
                    {
                        Debug.Log("Data array is null.");
                        return;
                    }
                    SpawnNetworkObject.instance.ClientId(playerData.clientId);
                    for (int i = 0; i < playerData.data.Count; i++)
                    {
                        SpawnNetworkObject.instance.AddPlayerAction(playerData.data[i]);
                    }
                    break;
                case "AddPlayerRoom":
                    DataReceivePlayerData newPlayerData = JsonConvert.DeserializeObject<DataReceivePlayerData>(jsonData);
                    if (newPlayerData.data == null)
                    {
                        Debug.Log("Data array is null.");
                        return;
                    }
                    for (int i = 0; i < newPlayerData.data.Count; i++)
                    {
                        SpawnNetworkObject.instance.AddPlayerAction(newPlayerData.data[i]);
                    }
                    break;
                case "Leave":
                    DataReceivePlayerData leavePlayerData = JsonConvert.DeserializeObject<DataReceivePlayerData>(jsonData);
                    if (leavePlayerData != null)
                    {
                        SpawnNetworkObject.instance.RemovePlayerAction(leavePlayerData.clientId);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }


    void OnWebSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e);
    }

    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket closed!");
    }
    void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
            ws.Close();
    }
}
public class DataItem
{
    public int id;
    public string name;
    public List<float> position;
    public Vector3 GetPosition()
    {
        return new(position[0], position[1], position[2]);
    }
}

public class DataReceive
{
    public string eventName;
}
public class DataReceivePlayerData
{
    public string eventName;
    public int clientId;
    public List<DataItem> data;
}