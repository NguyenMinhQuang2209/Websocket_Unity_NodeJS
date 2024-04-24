using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private NetworkObject network;
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    private PlayerAnimator playerAnimator;
    private void Start()
    {
        network = GetComponent<NetworkObject>();
        playerAnimator = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (!network.isOwner)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Movement(new Vector2(horizontal, vertical).normalized);
    }
    public void Movement(Vector2 input)
    {
        float speed = 0f;
        if (input.sqrMagnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(new(0f, input.x < 0f ? 180f : 0f, 0f));
            rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * input);

            PlayerNeworkManager.instance.PlayerMovement(transform.position, new(0f, input.x < 0f ? 180f : 0f, 0f));
            speed = 1f;
        }
        PlayerNeworkManager.instance.PlayerAnimator("Speed", speed);
    }
}
