using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer avatar;
    public void PlayerDataInit(RuntimeAnimatorController animator, Sprite newAvatar)
    {
        avatar.sprite = newAvatar;
        this.animator.runtimeAnimatorController = animator;
    }
}
