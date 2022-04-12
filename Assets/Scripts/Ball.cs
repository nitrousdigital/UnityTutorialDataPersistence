using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody ballRb;
    private Vector3 initialPosition;
    private Paddle paddle;

    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        paddle = FindObjectOfType <Paddle>();
        initialPosition = gameObject.transform.position;
    }

    public void ResetPosition()
    {
        ballRb.velocity = new Vector3(0, 0, 0);
        gameObject.transform.position = initialPosition;
        ballRb.transform.SetParent(paddle.transform);
    }

    private void OnCollisionExit(Collision other)
    {
        var velocity = ballRb.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        ballRb.velocity = velocity;
    }
}
