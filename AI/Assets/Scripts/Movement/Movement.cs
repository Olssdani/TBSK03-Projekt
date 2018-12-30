/*
    Script that handles the movement of the agents
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //Variables
    public float speed;
    private Rigidbody Rb;
    private Transform Tr;
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


    //Go closer to the target
    public bool GotoPos(Transform Target)
    {
        //Update the pathfing algoritm
        pathfinding.UpdateTarget(Target.position);
        List<Node> path = pathfinding.ReturnPath();
        if (path.Count == 0)
        {
            return true;
        }
        //If the agent is att the target
        if (CompleteNode())
        {
            Current = path[0];
        }

        float step = 3.0f * Time.deltaTime;
        //Find the target direction vector
        Vector3 targetDir = Current.worldPosition - transform.position;
        //Find a new vector that is rotated depending on the step
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        //Rotate the body
        transform.rotation = Quaternion.LookRotation(newDir);

        step = speed * Time.deltaTime;
        //If the angle is lower than 40 deegres move toward target
        if (Vector3.Angle(transform.forward, targetDir) < 40.0f)
        {
            transform.position = transform.position + transform.forward * step;
        }
        return false;
    }
    //Overloaded function
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
        if (Vector3.Angle(transform.forward, targetDir)<40.0f)
        {
            transform.position = transform.position + transform.forward * step;
        }
        return false;

    }

    //Rotates the agent without moving
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
    //Overloaded 
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

    //Get rotation
    public Quaternion GetRot()
    {
        return Rb.rotation;
    }

    //If the agent has completed a node
    bool CompleteNode()
    {
        if(Vector3.Distance(Current.worldPosition, transform.position) <0.2)
        {
            return true;
        }

        return false;
    }
}
