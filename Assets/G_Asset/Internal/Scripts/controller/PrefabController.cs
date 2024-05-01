using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PrefabController : MonoBehaviour
{
    public static PrefabController instance;
    [SerializeField] private List<WeaponConfig> weapons = new();
    private Dictionary<string, WeaponConfig> weaponStore = new();

    [SerializeField] private List<WeaponConfig> getWeaponsData = new();
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
        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponConfig currentWeapon = weapons[i];
            weaponStore[currentWeapon.ItemName] = weapons[i];
        }
        StartCoroutine(GetItemPrefab());
    }
    public IEnumerator GetItemPrefab()
    {
        string URL = API.instance.URL + "/item";
        using (UnityWebRequest www = new(URL, "GET"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.responseCode == 200)
            {
                List<WeaponConfig> weaponConfigs = JsonConvert.DeserializeObject<List<WeaponConfig>>(www.downloadHandler.text);
                for (int i = 0; i < weaponConfigs.Count; i++)
                {
                    WeaponConfig currentItem = weaponConfigs[i];
                    string weaponURL = currentItem.WeaponURL;
                    string bulletURL = currentItem.BulletURL;

                    yield return StartCoroutine(GetTexture(weaponURL, texture =>
                    {
                        currentItem.WeaponTexture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    }));

                    yield return StartCoroutine(GetTexture(bulletURL, texture =>
                    {
                        currentItem.BulletTexture = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    }));
                    getWeaponsData.Add(currentItem);
                }
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    public IEnumerator GetTexture(string url, Action<Texture2D> onDataRecived)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download texture: " + www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                onDataRecived?.Invoke(texture);
            }
        }
    }
    public WeaponConfig GetItem(string itemName)
    {
        return weaponStore[itemName];
    }
}
