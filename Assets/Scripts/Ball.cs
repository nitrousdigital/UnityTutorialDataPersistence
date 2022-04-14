using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody ballRb;
    private Vector3 initialPosition;
    private Paddle paddle;
    private float paddleWidth;
    private float halfPaddleWidth;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();    
        ballRb = GetComponent<Rigidbody>();
        paddle = FindObjectOfType <Paddle>();
        paddleWidth = paddle.GetComponent<BoxCollider>().size.x;
        halfPaddleWidth = paddleWidth / 2f;
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
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.5f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        
        ballRb.velocity = velocity;

        if (other.gameObject.CompareTag("Paddle"))
        {
            float paddleLeft = other.gameObject.transform.position.x - halfPaddleWidth;
            float impactX = Math.Max(ballRb.transform.position.x - paddleLeft, 0);
            if (impactX < halfPaddleWidth)
            {
                float leftPct = (halfPaddleWidth - impactX) / halfPaddleWidth;
                //Debug.Log("Ball collision x=" + ballRb.transform.position.x
                //        + " paddle.x=" + other.gameObject.transform.position.x
                //        + " LEFT=" + leftPct);
                ballRb.velocity = new Vector3(
                    -leftPct * 5f,
                    ballRb.velocity.y,
                    ballRb.velocity.z);
            }
            else
            {
                float rightPct = (impactX - halfPaddleWidth) / halfPaddleWidth;
                //Debug.Log("Ball collision x=" + ballRb.transform.position.x
                //        + " paddle.x=" + other.gameObject.transform.position.x
                //        + " RIGHT=" + rightPct);
                ballRb.velocity = new Vector3(
                    rightPct * 5f,
                    ballRb.velocity.y,
                    ballRb.velocity.z);
            }
            audioManager.PlayPaddleBounceSound();
        }
        else if (other.gameObject.CompareTag("Brick"))
        {
            audioManager.PlayBrickHitSound();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            audioManager.PlayWallBounceSound();
        }
    }
}
