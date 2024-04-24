using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNetworkObject : MonoBehaviour
{
    public static SpawnNetworkObject instance;
    public NetworkObject player;
    private Dictionary<int, NetworkObject> players = new();

    private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
    int clientId = -1;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void AddPlayerAction(DataItem dataItem)
    {
        if (players.ContainsKey(dataItem.id))
        {
            return;
        }
        _actions.Enqueue(() => SpawnPlayer(dataItem));
    }
    public void RemovePlayerAction(int id)
    {
        _actions.Enqueue(() => RemovePlayer(id));
    }
    private void Update()
    {
        if (_actions.Count > 0)
        {
            if (_actions.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }
    public void ClientId(int id)
    {
        if (clientId >= 0)
        {
            return;
        }
        clientId = id;
    }
    public void SpawnPlayer(DataItem dataItem)
    {
        if (players == null)
        {
            Debug.Log("Not player found");
            return;
        }
        if (players.ContainsKey(dataItem.id))
        {
            return;
        }

        NetworkObject tempPlayer = Instantiate(player, dataItem.GetPosition(), Quaternion.identity);
        tempPlayer.NetworkInit(dataItem.id, clientId == dataItem.id);
        players[dataItem.id] = tempPlayer;
    }
    public void RemovePlayer(int id)
    {
        NetworkObject removePlayer = players[id];
        players.Remove(id);
        if (removePlayer != null)
        {
            Destroy(removePlayer.gameObject);
        }
    }
    public int GetClientId()
    {
        return clientId;
    }
}