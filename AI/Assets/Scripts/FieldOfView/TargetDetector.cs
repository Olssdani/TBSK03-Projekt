/*
    This script detects targets within the viewradius.
    Mostly of this code i from Sebastian Lague with some minor changes made 
    by me to make it specific for this project.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour {

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask ObstacleMask;
    public List<Transform> VisableTargets = new List<Transform>();

    //this function is from Sebastian lague Youtube channel.
    public bool FindTarget()
    {
        //Empty the list
        VisableTargets.Clear();
        //See any targets in the targetmask is within a Sphere with the viewRadius
        Collider[] TargetsWithinCollider = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //Check all target within the sphere
        for(int i = 0; i< TargetsWithinCollider.Length; ++i)
        {
            //Create a vector between the target and the agent
            Transform target = TargetsWithinCollider[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            //Check if it is inside the viewangle
            if (Vector3.Angle(transform.forward, directionToTarget)<viewAngle/2)
            {
                float dist = Vector3.Distance(transform.position, target.position);
                //Check if the target is behind any objects.
                if (!Physics.Raycast(transform.position,directionToTarget,dist, ObstacleMask))
                {
                    VisableTargets.Add(target);
                    return true;
                }
            }
        }
        return false;
    }

    //this function is from Sebastian lague Youtube channel.
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
