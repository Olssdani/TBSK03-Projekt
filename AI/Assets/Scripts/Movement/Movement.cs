using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;
    private Rigidbody Rb;
    private Transform Tr;
    Vector3 movement;
    public float Rotate;
    public float force;
    // Use this for initialization
    void Start ()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        Tr = gameObject.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Rotate around axis
        //Rotate = Input.GetAxis("Horizontal");
        Vector3 m_EulerAngleVelocity = new Vector3(0, Rotate, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        Rb.MoveRotation(Rb.rotation * deltaRotation);
        //force = Input.GetAxis("Vertical");
        Rb.MovePosition(Tr.position + Tr.forward * force*speed);
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
