﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Random : MonoBehaviour {

    //Variable
    public float radius = 2;
    private Vector3 CurrentPos;
    private Vector3 MoveToPos;
    public float Speed = 2;

    bool first = true;

    public bool run()
    {
        if (first)
        {
            SetRandomPosition();
            first = false;
        }
        CurrentPos = gameObject.GetComponent<Transform>().position;

        Vector3 targetDirection = MoveToPos - CurrentPos;
        float step = Speed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);

        float angle = Vector3.Angle(gameObject.GetComponent<Transform>().forward, newDir);

        if (angle > 0.1)
        {
            gameObject.GetComponent<Movement>().Controller(0.0f, angle);
        }
        else
        {
      
            gameObject.GetComponent<Movement>().Controller(2.0f, 0.0f);
        }
        
        
        
        // Check if we have reached target
        if (Vector3.Distance(CurrentPos, MoveToPos)< 0.01f)
        {
            first = true;
            gameObject.GetComponent<Movement>().Controller(0.0f, 0.0f);
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetRandomPosition()
    {
        CurrentPos = gameObject.GetComponent<Transform>().position;
        float x = Random.Range(CurrentPos.x - radius, CurrentPos.x + radius);
        float z = Random.Range(CurrentPos.z - radius, CurrentPos.z + radius);
        MoveToPos = new Vector3(x, CurrentPos.y, z);
    }
}
