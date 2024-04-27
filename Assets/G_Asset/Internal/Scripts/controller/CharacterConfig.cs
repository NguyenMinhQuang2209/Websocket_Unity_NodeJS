using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterConfig : MonoBehaviour
{
    public static CharacterConfig instance;
    public PlayerData playerData;
    public List<CharacterItem> items = new();
    private Dictionary<string, CharacterItem> config = new();

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
    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            CharacterItem item = items[i];
            config[item.name] = item;
        }
    }
    public CharacterItem GetCharacter(string name)
    {
        return config[name];
    }
    public void CharacterInit(string name)
    {
        CharacterItem item = GetCharacter(name);
        if (item != null)
        {
            playerData.PlayerDataInit(item.animator, item.avatar);
        }
    }
    public int Left(int current)
    {
        if (current == 0)
        {
            current = items.Count - 1;
        }
        else
        {
            current--;
        }
        return current;
    }
    public int Right(int current)
    {
        if (current == items.Count - 1)
        {
            current = 0;
        }
        else
        {
            current++;
        }
        return current;
    }
    public Sprite GetCurrentSprite(int current)
    {
        return items[current].avatar;
    }
    public string GetCharacterName(int current)
    {
        return items[current].name;
    }
}

[System.Serializable]
public class CharacterItem
{
    public string name;
    public RuntimeAnimatorController animator;
    public Sprite avatar;
}