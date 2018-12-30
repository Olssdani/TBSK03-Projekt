/*
    This function build the mesh for the visited area

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryMesh : MonoBehaviour {

    //Variables
    Mesh mesh;
    public MeshFilter Out;
    Transform Tr;
    public Collider col;
    public float GridSize = 0.1f;
    Vector3 Size;
    bool [,] Map;
    int x = 0;
    int y = 0;
    bool updateMesh = false;
    int counter;
    Vector3[] vertices;


    // Use this for initialization
    void Start () {
        Tr = GetComponent<Transform>();
        Size = col.bounds.size;
        mesh = new Mesh();
        Out.mesh = mesh;
        x = (int)(Size.x / GridSize);
        y = (int)(Size.z / GridSize);
        Map = new bool[x, y];
        vertices = new Vector3[(x+1)*(y+1)];

        //Initialize the vertex
        for (int i = 0; i <= x; i++)
        {
            for (int j = 0; j <= y; j++)
            {
                if(j != y && i !=x )
                {
                    Map[i, j] = false;
                }
                Vector2 pos = Ind2World(i, j);
                vertices[i * x + j] = new Vector3(pos.x, 1.0f, pos.y);
               
            }
        }
        counter = 0;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        
        UpdateMap();
        //If the Map has changed then we update the mesh
        if(updateMesh)
        {
            MakeMesh();
            updateMesh = false;
        }
	}

    //Update the map when a new node has been visited
    void UpdateMap()
    {
        //Get the position from the player and the viewradius
        Vector2 pos = new Vector2(Tr.position.x, Tr.position.z);
        float viewRadius = GetComponent<FOW>().viewRadius;
        //Loop through all nodes in the grid
        for (int i = 0; i<x; i++)
        {
            for(int j = 0; j<y; j++)
            {
                //Make the node into worldcoordinates
                Vector2 temp = Ind2World(i, j);
               //Checks if it is outisde the viewradius
                if (Vector2.Distance(pos, temp)< viewRadius)
                {
                    //Check if it is visable for the player
                    if(!Physics.Raycast(Tr.position, new Vector3(temp.x, Tr.position.y,temp.y) - Tr.position, Vector3.Distance(new Vector3(temp.x, Tr.position.y, temp.y), Tr.position)))
                    {
                        //If already visited do nothing
                        if (!Map[i, j])
                        {
                            updateMesh = true;
                            Map[i, j] = true;
                            counter++;
                        }
                    }
                }
            }
        }
    }
    //Update the mesh
    void MakeMesh()
    {
        int[] triangles = new int[counter*6];
        int c = 0;
        //Loop throgh the logic grid
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // If the node is visited add that square to the mesh
                if (Map[i,j])
                {

                    triangles[c * 6] = i * x + j;
                    triangles[c * 6 + 1] = i * x + j + 1;
                    triangles[c * 6 + 2] = (i + 1) * x + j + 1;

                    triangles[c * 6 + 3] = (i + 1) * x + j + 1;
                    triangles[c * 6 + 4] = (i + 1) * x + j;
                    triangles[c * 6 + 5] = i * x + j;

                    c++;
                }
            }
        }
        //Update the mesh vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    //World coordinates to index
    Vector2Int World2Ind(Vector2 pos)
    {
        Vector2 normalPos;
        normalPos.x = (pos.x + Size.x / 2.0f) / Size.x;
        normalPos.y = (pos.y + Size.z / 2.0f) / Size.z;
        return new Vector2Int((int)(normalPos.x*x), (int)(normalPos.y * y));
    }
    //Index to World Coordinates
    Vector2 Ind2World(int i, int j)
    {
        Vector2 normalPos;
        normalPos.x = ((float)i /(float) x);
        normalPos.y = ((float)j / (float)y);
        Vector2 pos;
        pos.x = normalPos.x * Size.x - Size.x / 2;
        pos.y = normalPos.y * Size.z - Size.z / 2;
        return pos;
    }
}
