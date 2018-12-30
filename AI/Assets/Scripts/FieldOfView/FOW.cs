/*
    This scripts build the viewmesh. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOW : MonoBehaviour {

    //Variables
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle; 
    public int NrOfRays; 
    public LayerMask Obstacle; 
    public List<Vector3> rays = new List<Vector3>();
    public MeshFilter hide;
    Mesh mesh;
    private List<Vector3> Hits = new List<Vector3>();

    //Set the mesh for the area that is seen by the player
    void Start()
    {
        mesh = new Mesh();
        hide.mesh = mesh;
    }
   
    //Update after each frame. Its done with the physics calculations
    void LateUpdate()
    {
        SendRays();
        makeMesh();
    }

    //Send rays from the player
    void SendRays()
    {
        //Clear the list with the stored hitpoints and rays
        Hits.Clear();
        rays.Clear();
        //Get the angle between each ray
        float RayAngle = viewAngle/(float)(NrOfRays-1);
        //Get the start position for the first ray. It will start from left and go to the right
        Vector3 start = DirFromAngle(-viewAngle/2, false).normalized;
        //Loop over all rays
        for (int i = 0; i < NrOfRays ; ++i)
        {
            //Rotate the start ray depending on the angle between rays and which ray we are on.
            Vector3 ray = Quaternion.AngleAxis(RayAngle*i, Vector3.up) * start;
            RaycastHit HitPoint;

            if (Physics.Raycast(transform.position, ray, out HitPoint, viewRadius, Obstacle))
            {
                //If we hit a object store that ray and hit position
                rays.Add(HitPoint.point - transform.position);
                Hits.Add(HitPoint.point);
            }
            else
            {
                //If we don't hit any collider we store the point on the end of the ray
                rays.Add((transform.position + ray * viewRadius) - transform.position);
                Hits.Add((ray * (viewRadius)+ transform.position));
            }
        }
    }

    //Build the mesh over the viewarea
    void makeMesh()
    {
        //Variables
        Vector3[] vertices = new Vector3[NrOfRays+1];
        int[] triangles = new int[(NrOfRays-1)*3];
        //Set the players position to the first vertices
        vertices[0] = transform.position;
        //Loop over the hits and set that position to a vertices
        int i = 1;
        foreach(Vector3 p in Hits)
        {
            vertices[i] = p;
            i++;
        }

        //Build the triangle mesh from the vertexes. Every triangle gets the first vertices as the players position
        for (int j = 0; j < NrOfRays-1; j++ )
        {
            triangles[j * 3] = 0;
            triangles[j * 3 + 1] = j + 1;
            triangles[j * 3 + 2] = j + 2;
        }
        //Set the new Vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    //this function is from Sebastian lague Youtube channel.
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
