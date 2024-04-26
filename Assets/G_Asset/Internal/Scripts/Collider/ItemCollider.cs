using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<NetworkObject>(out var networkObject) && collision.GetComponent<PlayerMovement>() != null)
        {
            if (networkObject.isOwner)
            {
                Debug.Log("Hit owner");
            }
        }
    }
}
