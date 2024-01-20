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
    [SerializeField] private float extraJumpForce;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform groundCheck;
    private bool isGrounded;
    private Vector2 moveDirection;
    private bool canDash = true;
    private bool jumpControl;
    private int jumpIteration = 0;
    private int jumpValueIteration = 60;


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

        Jump();
    }

    private void Update()
    {
        Dash(moveDirection);
        ExtraJump();
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumpControl = true;
                canDash = true;
            }
        }
        else
        {
            jumpControl = false;
        }

        if (jumpControl)
        {
            if (jumpIteration++ < jumpValueIteration)
            {
                rb.AddForce(Vector2.up * jumpForce / jumpIteration);
            }
        }
        else
        {
            jumpIteration = 0;
        }
    }

    private void ExtraJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isGrounded)
            {
                rb.AddForce(Vector2.up * extraJumpForce);
            }
        }
    }


    private void Dash(Vector2 moveDirection)
    {
        if (!canDash)
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (rb.velocity.x >= 0)
                rb.AddForce(Vector2.right * (dashForce));

            else
                rb.AddForce(Vector2.left * (dashForce));


            if (!isGrounded)
                canDash = false;
        }
    }
}