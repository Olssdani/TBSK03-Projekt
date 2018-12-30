/*
This script creates the nodes which is used in the GridScript class.
Mostly of this code i from Sebastian Lague with some minor changes made 
by me to make it specific for this project.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    //Contructor
    public Node(bool _walkabale, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkabale;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    //Gets the F cost for the node which is the G +H cost.
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //Sets the node to be walkable or not.
    public void SetWalkable(bool able)
    {
        walkable = able;
    }
}
