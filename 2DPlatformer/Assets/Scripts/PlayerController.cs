using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform groundCheck;
    private bool isGrounded;
    private int extraJumpCount = 0;
    private Vector2 moveDirection;
    private bool canDash = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);


        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash(moveDirection);
        }

        if (isGrounded)
        {
            extraJumpCount = 0;
            canDash = true;
        }

        if (extraJumpCount < 1 && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extraJumpCount++;
        }
    }

    private void Dash(Vector2 moveDirection)
    {
        if (!canDash)
            return;

        if (rb.velocity.x >= 0)
            rb.AddForce(Vector2.right * (dashForce * 100));

        else
            rb.AddForce(Vector2.left * (dashForce * 100));


        if (!isGrounded)
            canDash = false;
    }
}