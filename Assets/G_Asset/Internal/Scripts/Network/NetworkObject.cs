using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    [HideInInspector] public int networkId;
    [HideInInspector] public bool isOwner = false;
    public void NetworkInit(int id, bool isOwner)
    {
        networkId = id;
        this.isOwner = isOwner;
    }
}
