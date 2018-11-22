﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;
    private Rigidbody Rb;
    private Transform Tr;
    Vector3 movement;
    public float Rotate;
    public float force;
    public GameObject Astar;
    Pathfinding pathfinding;

    // Use this for initialization
    void Start ()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        Tr = gameObject.GetComponent<Transform>();
        pathfinding = Astar.GetComponent<Pathfinding>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 m_EulerAngleVelocity = new Vector3(0, Rotate, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        Rb.MoveRotation(Rb.rotation * deltaRotation);
        Rb.MovePosition(Tr.position + Tr.forward * force*speed);
        force = 0;
        Rotate = 0;
    }


    public void GotoPos(Vector3 Position)
    {
        pathfinding.UpdateTarget(Position);
        List<Node> path = pathfinding.ReturnPath();

        float angle = Vector3.Angle(Tr.forward, Position - Tr.position);
        if (angle > 0.01)
        {
            Rotate = 2.0f;
        }
        

    }
   public void Controller(float F, float R)
    {
        force = F;
        Rotate = R;
    }

    public Quaternion GetRot()
    {
        return Rb.rotation;
    }
}
