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


    public void GotoPos(Transform Target)
    {
        pathfinding.UpdateTarget(Target.position);
        List<Node> path = pathfinding.ReturnPath();

       // float angle = Vector3.Angle(Tr.forward, Position - Tr.position);


        transform.LookAt(path[0].worldPosition);

        float step = speed * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, Target.position, step);

        // Rotate our transform a step closer to the target's.
        //Quaternion targetRotation = Quaternion.LookRotation(-Position, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        // Vector3 newDir = Vector3.RotateTowards(transform.forward, Position - Tr.position, step, 0.0f);
        //Debug.DrawRay(transform.position, newDir, Color.red);
        //Tr.rotation = Quaternion.LookRotation(newDir);

        /*if (angle > 0.01)
        {
            Rotate = 2.0f;
        }*/


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
