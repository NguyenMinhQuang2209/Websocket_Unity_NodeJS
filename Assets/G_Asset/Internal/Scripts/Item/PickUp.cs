using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactible
{
    [SerializeField] private Item item;
    public override void Interact(GameObject target)
    {
        if (target.TryGetComponent<PlayerEquipment>(out var playerEquipment))
        {
            playerEquipment.PickUpItem(item);
        }
    }
}
