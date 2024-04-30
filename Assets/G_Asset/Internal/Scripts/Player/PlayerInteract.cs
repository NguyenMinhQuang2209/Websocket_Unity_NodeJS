using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private NetworkObject network;
    private void Start()
    {
        network = GetComponent<NetworkObject>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!network.isOwner)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent<Interactible>(out var interactible))
        {
            interactible.BaseInteract(gameObject);
        }
    }
}
