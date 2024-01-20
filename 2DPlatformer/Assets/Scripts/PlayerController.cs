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
    [SerializeField] private GameObject tp;
    [SerializeField] private Transform tpHolder;

    private int extraJumpCount = 0;
    private GameObject instantiatedTP;
    private Rigidbody2D rb;
    private Transform groundCheck;
    private bool isGrounded;
    private Vector2 moveDirection;
    private bool canDash = true;
    private bool jumpControl;
    private int jumpIteration = 0;
    private int jumpValueIteration = 60;
    [SerializeField] private bool tpThrown;
    private bool faceRight = true;


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
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
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
            if (!isGrounded && extraJumpCount++ < 1)
            {
                rb.AddForce(Vector2.up * extraJumpForce);
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
}