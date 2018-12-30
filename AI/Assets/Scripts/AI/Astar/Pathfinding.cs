/*
This script takes a start and end position and calculates the shorters path using A*.
Mostly of this code i from Sebastian Lague with some minor changes made 
by me to make it specific for this project.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    //Variables
    GridScript grid;
    private Vector3 target;
    public Transform seeker;

    //Sets the value of yje vairables.
    void Awake()
    {
        grid = GetComponent<GridScript>();
        target = seeker.position;
        FindPath(seeker.position, target);

    }
	
    //This function is called when the target has moved and recalculate the new path.
    public void UpdateTarget(Vector3 Position)
    {
        target = Position;
        FindPath(seeker.position, target);
    }
    //If no target is selected, for example if the seeker is still.
    public void NoTarget()
    {
        target = seeker.position ;
    }

    //Return the shortest path
    public List<Node> ReturnPath()
    {
        return grid.path;
    }
    //Return the grid
    public GridScript ReturnGrid()
    {
        return grid;
    }

    //This function calculates the shortest path using A*
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Get the start node and end node from their world position.
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);


        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        //Add the start node to the openset
        openSet.Add(startNode);

        //Loop until the openset is empty
        while(openSet.Count>0)
        {
            Node currentNode = openSet[0];
            //Find the node with the lowest cost
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost< currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            //Remove the node from the openset to the closed set.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //If the current node it the end
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            // Add the neighbours to the openset
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
               
                //Make the node not go diagonaly when a neighbour is unwalkable "this is code by me".
                if (GetDistance(currentNode, neighbour)>10)
                {
                    int a = neighbour.gridX - currentNode.gridX;
                    int b = neighbour.gridY - currentNode.gridY;

                    if(!grid.getNode(neighbour.gridX-a, neighbour.gridY).walkable || !grid.getNode(neighbour.gridX, neighbour.gridY-b).walkable)
                    {
                        continue;
                    }
                }

                int newMocementCOstToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                //Update H and G costs
                if (newMocementCOstToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMocementCOstToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    //Add the neighbours to the openset
                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    //Retrace the path and set each nodes parent
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    //Calulate the distance using chessboard distance. 
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX>dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
