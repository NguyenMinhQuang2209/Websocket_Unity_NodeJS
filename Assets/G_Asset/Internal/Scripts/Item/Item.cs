
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemName itemName;
    public ItemType GetItemType()
    {
        return itemType;
    }
    public ItemName GetName()
    {
        return itemName;
    }
}
