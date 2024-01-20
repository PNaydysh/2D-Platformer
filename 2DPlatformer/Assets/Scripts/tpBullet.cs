using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpBullet : MonoBehaviour
{
    private float speed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }
}