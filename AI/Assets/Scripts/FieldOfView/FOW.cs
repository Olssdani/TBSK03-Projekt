using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOW : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle; 
    public int NrOfRays;
    
    public LayerMask Obstacle; 
    public List<Vector3> rays = new List<Vector3>();
    public List<Vector3> Orginalrays = new List<Vector3>();
    public MeshFilter hide;
    public float Offset;
    Mesh mesh;
    private List<Vector3> Hits = new List<Vector3>();
    void Start()
    {
        mesh = new Mesh();
        hide.mesh = mesh;
    }
   
    void LateUpdate()
    {
        SendRays();
        makeMesh();
    }

    void SendRays()
    {
        Hits.Clear();
        rays.Clear();
        Orginalrays.Clear();
        float RayAngle = viewAngle/(float)(NrOfRays-1);
        Vector3 start = DirFromAngle(-viewAngle/2, false).normalized;
        for (int i = 0; i < NrOfRays ; ++i)
        {
            Vector3 ray = Quaternion.AngleAxis(RayAngle*i, Vector3.up) * start;
            RaycastHit HitPoint;

            if (Physics.Raycast(transform.position, ray, out HitPoint, viewRadius, Obstacle))
            {
                rays.Add(HitPoint.point - transform.position);
                Hits.Add(HitPoint.point);
           
            }
            else
            {
                rays.Add((transform.position + ray * viewRadius) - transform.position);
                Hits.Add((ray * (viewRadius)+ transform.position));
            }
        }
    }

    void makeMesh()
    {
        Vector3[] vertices = new Vector3[NrOfRays+1];
        int[] triangles = new int[(NrOfRays-1)*3];
        vertices[0] = transform.position;
        int i = 1;
        foreach(Vector3 p in Hits)
        {
            vertices[i] = p;
            i++;
        }

        for(int j = 0; j < NrOfRays-1; j++ )
        {
            triangles[j * 3] = 0;
            triangles[j * 3 + 1] = j + 1;
            triangles[j * 3 + 2] = j + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
