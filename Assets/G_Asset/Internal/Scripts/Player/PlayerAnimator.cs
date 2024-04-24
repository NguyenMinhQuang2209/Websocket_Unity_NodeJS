using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }
    public void SetBool(string name, bool v)
    {
        animator.SetBool(name, v);
    }
}
