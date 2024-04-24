using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeworkManager : MonoBehaviour
{
    public static PlayerNeworkManager instance;
    private Dictionary<int, NetworkObject> players = new();
    private List<PlayerListItem> playersList = new();


    public struct PlayerPosition
    {
        public string eventName;
        public int clientId;
        public float[] position;
        public float[] rotation;
        public PlayerPosition(string eventName, int clientId, float[] position, float[] rotation)
        {
            this.eventName = eventName;
            this.clientId = clientId;
            this.position = position;
            this.rotation = rotation;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void AddPlayer(int id, NetworkObject player)
    {
        players[id] = player;
        playersList.Add(new(player, id));
    }
    public void RemovePlayer(int id)
    {
        players.Remove(id);
        for (int i = 0; i < playersList.Count; i++)
        {
            PlayerListItem item = playersList[i];
            if (item.id == id)
            {
                playersList.RemoveAt(i);
                break;
            }
        }
    }
    public void PlayerMovement(Vector3 newPos, Vector3 newRot)
    {
        float[] pos = new float[] { newPos.x, newPos.y, newPos.z };
        float[] rot = new float[] { newRot.x, newRot.y, newRot.z };

        int clientId = SpawnNetworkObject.instance.GetClientId();
        PlayerPosition newPosition = new("Movement", clientId, pos, rot);

        string jsonData = JsonConvert.SerializeObject(newPosition);

        Ws_Client.instance.ws.Send(jsonData);
    }
    public void PlayerAnimator(string name, float v)
    {
        int clientId = SpawnNetworkObject.instance.GetClientId();
        PlayerKeyValue keys = new("PlayerAnimator", clientId, name, v);
        string keyJsonData = JsonConvert.SerializeObject(keys);

        Ws_Client.instance.ws.Send(keyJsonData);
    }
    public void Movement(DataReceivePlayerData data)
    {
        int clientId = SpawnNetworkObject.instance.GetClientId();
        if (clientId == data.clientId)
        {
            return;
        }

        for (int i = 0; i < playersList.Count; i++)
        {
            PlayerListItem player = playersList[i];
            if (player.id == data.clientId)
            {
                NetworkObject currentPlayer = player.player;
                DataItem dataItem = data.data[0];
                currentPlayer.transform.SetPositionAndRotation(new(dataItem.position[0], dataItem.position[1], dataItem.position[2]), Quaternion.Euler(new(dataItem.rotation[0], dataItem.rotation[1], dataItem.rotation[2])));
                break;
            }
        }
    }
    public void Animator(PlayerKeyValue data)
    {
        for (int i = 0; i < playersList.Count; i++)
        {
            PlayerListItem player = playersList[i];
            if (player.id == data.clientId)
            {
                NetworkObject currentPlayer = player.player;
                if (currentPlayer.TryGetComponent<PlayerAnimator>(out var playerAnimator))
                {
                    playerAnimator.SetFloat(data.key, data.value);
                }
                break;
            }
        }
    }
}
public class PlayerListItem
{
    public NetworkObject player;
    public int id;
    public PlayerListItem(NetworkObject player, int id)
    {
        this.player = player;
        this.id = id;
    }
}

public class PlayerKeyValue
{
    public string eventName;
    public int clientId;
    public string key;
    public float value;
    public PlayerKeyValue(string eventName, int clientId, string key, float value)
    {
        this.eventName = eventName;
        this.key = key;
        this.clientId = clientId;
        this.value = value;
    }
}