
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] private ItemType itemType;
    public ItemType GetItemType()
    {
        return itemType;
    }
}
