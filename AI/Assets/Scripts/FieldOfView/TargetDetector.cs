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

    public bool FindTarget()
    {
        VisableTargets.Clear();
        Collider[] TargetsWithinCollider = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i< TargetsWithinCollider.Length; ++i)
        {
          
            Transform target = TargetsWithinCollider[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, directionToTarget)<viewAngle/2)
            {
                float dist = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position,directionToTarget,dist, ObstacleMask))
                {
                   
                    VisableTargets.Add(target);
                    return true;
                }
            }
        }
        return false;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
