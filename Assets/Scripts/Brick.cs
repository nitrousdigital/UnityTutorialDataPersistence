using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    private int PointValue = 1;

    private MaterialPropertyBlock block;

    void Start()
    {
        UpdateColor();
    }

    public void SetPointValue(int points)
    {
        PointValue = points;
        UpdateColor();
    }

    private void UpdateColor()
    {
        Renderer brickRenderer = GetComponentInChildren<Renderer>();
        block = new MaterialPropertyBlock();

        switch (PointValue)
        {
            case 1:
                block.SetColor("_BaseColor", Color.green);
                break;
            case 2:
            case 3:
                block.SetColor("_BaseColor", Color.yellow);
                break;
            case 4:
            case 5:
                block.SetColor("_BaseColor", Color.blue);
                break;
            default:
                block.SetColor("_BaseColor", Color.red);
                break;
        }
        brickRenderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        // delay hiding the brick to ensure
        // the ball will have time to bounce
        StartCoroutine(ScheduleHide());
    }

    private IEnumerator ScheduleHide()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        onDestroyed.Invoke(PointValue);
    }
}
