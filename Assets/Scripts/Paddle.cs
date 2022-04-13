using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float Speed = 2.0f;
    public float MaxMovement = 2.0f;

    private float initialX;
    private MainManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

        initialX = transform.position.x;
        gameManager = FindObjectOfType<MainManager>();
    }

    /**
     * Reset the paddle back to its initial position
     */
    public void ResetPosition()
    {
        Vector3 pos = transform.position;
        pos.x = initialX;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsPlaying())
        {
            return;
        }

        float input = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += input * Speed * Time.deltaTime;

        if (pos.x > MaxMovement)
        {
            pos.x = MaxMovement;
        }            
        else if (pos.x < -MaxMovement)
        {
            pos.x = -MaxMovement;
        }
            
        transform.position = pos;
    }
}
