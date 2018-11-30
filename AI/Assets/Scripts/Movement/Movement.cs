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
    Node Current;

    // Use this for initialization
    void Start ()
    {
        Rb = gameObject.GetComponent<Rigidbody>();
        Tr = gameObject.GetComponent<Transform>();
        pathfinding = Astar.GetComponent<Pathfinding>();
        Current = pathfinding.ReturnGrid().NodeFromWorldPoint(transform.position);
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


    public bool GotoPos(Transform Target)
    {
        pathfinding.UpdateTarget(Target.position);
        List<Node> path = pathfinding.ReturnPath();
        if (path.Count == 0)
        {
            return true;
        }

        float step = 3.0f * Time.deltaTime;
       
        Vector3 targetDir = Target.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        step = speed * Time.deltaTime;
        // Move our position a step closer to the target.
        if (Vector3.Angle(transform.forward, targetDir) < 30.0f)
        {
            transform.position = transform.position+ transform.forward * step;
        }
        return false;
    }

    public bool GotoPos(Vector3 Target)
    {
        pathfinding.UpdateTarget(Target);
        List<Node> path = pathfinding.ReturnPath();
        if(path.Count == 0)
        {
            return true;
        }
        if(CompleteNode())
        {
            Current = path[0];
        }

        float step = 3.0f * Time.deltaTime;

        Vector3 targetDir = Current.worldPosition - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        step = speed * Time.deltaTime;
        if (Vector3.Angle(transform.forward, targetDir)<30.0f)
        {
            transform.position = transform.position + transform.forward * step;
        }
        return false;

    }

    public bool LookAt(float Angle)
    {
        float step = 3.0f * Time.deltaTime;

        Vector3 targetDir = (transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad*Angle),0, Mathf.Cos(Mathf.Deg2Rad * Angle))) - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        if (Vector3.Angle(transform.forward, newDir) < 0.5)
        {
            transform.rotation = Quaternion.LookRotation(newDir);
            return true;
        }
        transform.rotation = Quaternion.LookRotation(newDir);

        return false;
    }

    public bool LookAt(Vector3 pos)
    {
        float step = 3.0f * Time.deltaTime;

        Vector3 targetDir = pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        if(Vector3.Angle(transform.forward, newDir )<0.5)
        {
            transform.rotation = Quaternion.LookRotation(newDir);
            return true;
        }
        transform.rotation = Quaternion.LookRotation(newDir);

        return false;
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

    bool CompleteNode()
    {
        if(Vector3.Distance(Current.worldPosition, transform.position) <0.1)
        {
            return true;
        }

        return false;
    }
}
