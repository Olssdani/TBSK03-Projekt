/*
This script handle de grid of nodes that is used in the A* algoritm.
Mostly of this code i from Sebastian Lague with some minor changes made 
by me to make it specific for this project.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    //Variable
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public List<Node> path;
    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    //Set all variables
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //Create Grid fill it with nodes
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -Vector3.forward*gridWorldSize.y/2;

        for(int x =0; x< gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                grid[x, y] = new Node(true, worldPoint, x, y);
            }
        }
    }
    //Return gridsize in X
    public int GetSizeX()
    {
        return gridSizeX;

    }
    //Return gridsize in Y
    public int GetSizeY()
    {
        return gridSizeY;

    }
    //Return a spefic node
    public Node getNode(int x, int y)
    {
        return grid[x,y];
    }

    //Get Neighbours to a node
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> Neighbour = new List<Node>();
        for(int x=-1; x<=1; x++)
        {
            for(int y = -1; y<=1; y++ )
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >=0 && checkX < gridSizeX && checkY >=0 && checkY < gridSizeY)
                {
                    Neighbour.Add(grid[checkX, checkY]);
                }
            }
        }
        return Neighbour;
    }

    //Get the node which a worldposition lies within.
    public Node NodeFromWorldPoint( Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    //This function draws the A* path in the editor.
   void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid!= null && Application.isPlaying)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(path!=null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.01f));
            }
        }
    }
}