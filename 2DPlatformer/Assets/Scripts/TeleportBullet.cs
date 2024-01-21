using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBullet : MonoBehaviour
{
    private float teleportBulletSpeed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * teleportBulletSpeed;
    }
}