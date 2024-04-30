using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    private NetworkObject networkObject;
    private void Start()
    {
        networkObject = GetComponent<NetworkObject>();
    }
    public void SetFloat(string name, float value, bool useDirectly = false)
    {
        if (networkObject.isOwner && !useDirectly)
        {
            return;
        }
        animator.SetFloat(name, value);
    }
    public void SetBool(string name, bool v)
    {
        animator.SetBool(name, v);
    }
}
