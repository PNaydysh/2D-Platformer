using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement forces")] [SerializeField]
    private float moveSpeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float extraJumpForce;
    [SerializeField] private float wallSliddingSpeed;

    [Header("Ground check settings")] [SerializeField]
    private bool isGrounded;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [Header("Wall check settings")] [SerializeField]
    private bool isWallSliding;

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;

    [Header("Teleport settings")] [SerializeField]
    private bool tpThrown;

    [SerializeField] private GameObject tp;
    [SerializeField] private Transform tpHolder;

    private int extraJumpCount = 0;
    private int jumpIteration = 0;
    private int jumpValueIteration = 60;

    private bool canDash = true;
    private bool jumpControl = true;
    private bool faceRight = true;

    private GameObject instantiatedTP;

    private Rigidbody2D rb;

    private Vector2 moveDirection;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);


        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer);
        if (isGrounded)
        {
            extraJumpCount = 0;
            canDash = true;
        }

        Jump();
    }

    private void Update()
    {
        Dash();
        ExtraJump();
        Flip();
        ThrowTP();
        WallSlide();
        WallJump();
        GetDown();
    }

    private void GetDown()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(9, 8, true);
            Invoke("ActivateCollision", 0.5f);
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumpControl = true;
            }
            else if (IsWalled())
            {
                isWallSliding = false;
                jumpControl = true;
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
            if (!isGrounded && !IsWalled() && extraJumpCount++ < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * extraJumpForce, ForceMode2D.Impulse);
            }
        }
    }


    private void Dash()
    {
        if (!canDash)
            return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (faceRight)
                rb.AddForce(Vector2.right * (dashForce));

            else if (!faceRight)
                rb.AddForce(Vector2.left * (dashForce));


            if (!isGrounded)
                canDash = false;
        }
    }

    private void Flip()
    {
        if ((rb.velocity.x > 0 && !faceRight) || (rb.velocity.x < 0 && faceRight))
        {
            transform.Rotate(0f, 180f, 0f);
            faceRight = !faceRight;
        }
    }

    private void ThrowTP()
    {
        if (Input.GetKeyDown(KeyCode.E) && !tpThrown)
        {
            instantiatedTP = Instantiate(tp, tpHolder.position, tpHolder.rotation);
            tpThrown = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && tpThrown)
        {
            transform.position = instantiatedTP.transform.transform.position;
            Destroy(instantiatedTP);
            tpThrown = false;
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSliddingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsWalled())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            if (faceRight)
            {
                rb.AddForce(new Vector2(-jumpForce, jumpForce));
            }
            else
            {
                rb.AddForce(new Vector2(jumpForce, jumpForce));
            }

            canDash = true;
            isWallSliding = false;
        }
    }

    private void ActivateCollision()
    {
        Physics2D.IgnoreLayerCollision(9, 8, false);
    }
}