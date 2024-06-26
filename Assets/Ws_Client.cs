using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using System;
public class Ws_Client : MonoBehaviour
{
    public WebSocket ws;
    [SerializeField] private string ipAddress = "ws://localhost:3000";
    public static Ws_Client instance;
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
        ws = new WebSocket(ipAddress);
        ws.OnOpen += (sender, e) => OnWebSocketOpen(sender, e);
        ws.OnMessage += (sender, e) => OnMessageReceived(sender, e);
        ws.OnError += (sender, e) => OnWebSocketError(sender, e);
        ws.OnClose += (sender, e) => OnWebSocketClose(sender, e);

        ws.Connect();
    }
    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        string characterName = PlayerPrefs.GetString("characterName");
        string username = PlayerPrefs.GetString("username");
        DataItem item = new("JoinServer", username, characterName, new() { 0, 0, 0 }, new() { 0, 0, 0 });
        string jsonData = JsonConvert.SerializeObject(item);
        ws.Send(jsonData);
    }

    void OnMessageReceived(object sender, MessageEventArgs e)
    {
        string jsonData = e.Data;
        try
        {
            DataReceive dataReceive = JsonUtility.FromJson<DataReceive>(jsonData);
            string[] eventsName = dataReceive.eventName.Split("-");
            if (eventsName.Length > 1)
            {
                string target = eventsName[0];
                string events = eventsName[1];
                switch (target)
                {
                    case "Player":
                        PlayerAction(events, jsonData);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public void PlayerAction(string eventName, string jsonData)
    {
        switch (eventName)
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
            case "Movement":
                DataReceivePlayerData playerMovementData = JsonConvert.DeserializeObject<DataReceivePlayerData>(jsonData);
                SpawnNetworkObject.instance.AddMovementAction(playerMovementData);
                break;
            case "PlayerAnimator":
                PlayerKeyValue animatorData = JsonConvert.DeserializeObject<PlayerKeyValue>(jsonData);
                SpawnNetworkObject.instance.AddAnimatorAction(animatorData);
                break;
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
    public string eventName;
    public int id;
    public string name;
    public string characterName;
    public List<float> position;
    public List<float> rotation;
    public Vector3 GetPosition()
    {
        return new(position[0], position[1], position[2]);
    }
    public Vector3 GetRotation()
    {
        return new(rotation[0], rotation[1], rotation[2]);
    }
    public DataItem(string eventName, string name, string characterName, List<float> position, List<float> rotation)
    {
        this.eventName = eventName;
        this.name = name;
        this.characterName = characterName;
        this.position = position;
        this.rotation = rotation;
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