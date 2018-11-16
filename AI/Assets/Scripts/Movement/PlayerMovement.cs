using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float SpeedMovement;
    public float SpeedRotation;
    private Rigidbody Rb;
    private Transform Tr;
    Vector3 movement;
    private float Rotate;
    private float Force;
    // Use this for initialization
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        Tr = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Force = Input.GetAxis("Vertical");
        Rotate = Input.GetAxis("Horizontal");


        //Rotate around axis
        Vector3 m_EulerAngleVelocity = new Vector3(0, Rotate* SpeedRotation, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        Rb.MoveRotation(Rb.rotation * deltaRotation);
        Rb.MovePosition(Tr.position + Tr.forward * Force* SpeedMovement);
    }

}

