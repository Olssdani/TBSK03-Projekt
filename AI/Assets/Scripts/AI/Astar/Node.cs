﻿using System.Collections;
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

    public Node(bool _walkabale, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkabale;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }


    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}