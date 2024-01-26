using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 3f;

    private Vector2 targetPos;

    private void Start()
    {
        targetPos = endPoint.position;
    }

    private void FixedUpdate()
    {
        if (transform.position == startPoint.position)
        {
            targetPos = endPoint.position;
        }
        
        if (transform.position == endPoint.position)
        {
            targetPos = startPoint.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }


    private void OnDrawGizmos()
    {
        if(startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
    }
}
