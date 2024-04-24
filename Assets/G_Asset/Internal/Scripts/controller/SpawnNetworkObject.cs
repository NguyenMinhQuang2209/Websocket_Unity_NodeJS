using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNetworkObject : MonoBehaviour
{
    public static SpawnNetworkObject instance;
    public NetworkObject player;
    private Dictionary<int, NetworkObject> objects = new();

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
        if (objects.ContainsKey(dataItem.id))
        {
            return;
        }
        _actions.Enqueue(() => SpawnPlayer(dataItem));
    }
    public void RemovePlayerAction(int id)
    {
        _actions.Enqueue(() => RemovePlayer(id));
    }
    public void AddMovementAction(DataReceivePlayerData data)
    {
        _actions.Enqueue(() => MovementPlayer(data));
    }
    public void AddAnimatorAction(PlayerKeyValue data)
    {
        _actions.Enqueue(() => AnimatorPlayer(data));
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
        if (objects == null)
        {
            Debug.Log("Not player found");
            return;
        }
        if (objects.ContainsKey(dataItem.id))
        {
            return;
        }

        NetworkObject tempPlayer = Instantiate(player, dataItem.GetPosition(), Quaternion.Euler(dataItem.GetRotation()));
        tempPlayer.NetworkInit(dataItem.id, clientId == dataItem.id);
        objects[dataItem.id] = tempPlayer;
        PlayerNeworkManager.instance.AddPlayer(dataItem.id, tempPlayer);
    }
    public void RemovePlayer(int id)
    {
        NetworkObject removePlayer = objects[id];
        objects.Remove(id);
        PlayerNeworkManager.instance.RemovePlayer(id);
        if (removePlayer != null)
        {
            Destroy(removePlayer.gameObject);
        }
    }
    public void MovementPlayer(DataReceivePlayerData data)
    {
        PlayerNeworkManager.instance.Movement(data);
    }
    public void AnimatorPlayer(PlayerKeyValue data)
    {
        PlayerNeworkManager.instance.Animator(data);
    }
    public int GetClientId()
    {
        return clientId;
    }
}