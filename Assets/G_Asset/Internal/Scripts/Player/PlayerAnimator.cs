using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }
    public void SetBool(string name, bool v)
    {
        animator.SetBool(name, v);
    }
}
