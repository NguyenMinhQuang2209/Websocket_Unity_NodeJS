using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private NetworkObject network;
    public CinemachineVirtualCamera mainCamera;
    private void Start()
    {
        network = GetComponent<NetworkObject>();
        if (!network.isOwner)
        {
            mainCamera.Priority = 0;
            mainCamera.enabled = false;
        }
        else
        {
            mainCamera.Priority = 1;
            mainCamera.enabled = true;
        }
        if (BoundController.instance != null)
        {
            ChangeCameraBound(BoundController.instance.bound);
        }
    }
    public void ChangeCameraBound(Collider2D bound)
    {
        if (mainCamera.TryGetComponent<CinemachineConfiner2D>(out var mainCameraConfiner))
        {
            mainCameraConfiner.m_BoundingShape2D = bound;
        }
    }
}
