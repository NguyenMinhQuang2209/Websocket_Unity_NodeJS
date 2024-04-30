using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Transform equipment;
    private NetworkObject network;
    private Item currentItem = null;
    private void Start()
    {
        network = GetComponent<NetworkObject>();
    }
    private void Update()
    {
        if (!network.isOwner)
        {
            return;
        }
        Vector2 mousePos = Input.mousePosition;
        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 targetPos = mousePos - playerPos;

        float yAxis = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

        float splitYAxis = yAxis <= 90f && yAxis >= -90f ? 0f : 180f;

        if (splitYAxis == 180f)
        {
            yAxis = -yAxis;
        }

        equipment.transform.rotation = Quaternion.Euler(new(splitYAxis, 0f, yAxis));
    }
    public void PickUpItem(Item item)
    {
        if (currentItem == null)
        {
            currentItem = item;
        }

        switch (item.GetItemType())
        {
            case ItemType.Equipment:
                break;
        }
    }
}
