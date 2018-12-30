/*
This script updates the wallmap which is used for the pathfining algoritm

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallMap : MonoBehaviour {

    //Variables
    public Image Im;
    Vector2 size;
    public GameObject player;
    private float[,] Map;
    public Collider col;
    float groundSize = 0;
    GridScript grid;
    bool status;

    // Use this for initialization
    void Start()
    {
        grid = GetComponent<Pathfinding>().ReturnGrid();
        size = new Vector2Int(grid.GetSizeX(), grid.GetSizeY());
        Map = new float[(int)size.x, (int)size.y];
        InitArray();
        groundSize = col.bounds.size.x;
        status = false;
    }
    // Update is called once per frame
    void Update()
    {
        //Update map 
        UpdateMap();
        //Only update the mesh if the map has changed
        if (status)
        {
            UpdateGrid();
            status = false;
        }
        //Update the image in the editor
        ShowUpdate(Map);
    }

    void UpdateMap()
    {
        //Get variable from the player
        Vector3 pos = player.transform.position;
        Vector2 NormalPos = World2Normal(pos);
        TargetDetector script = player.GetComponent<TargetDetector>();
        RaycastHit Hitpoint;
        //Loop thorugh the size of the map
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector3 WorldPos = Indices2World(x, y);
                //Check if the pos is outside of the view radius
                if (Vector3.Distance(player.transform.position, WorldPos) < script.viewRadius)
                {
                    //Check if there is something that intersepts the ray from the agent to the pos.
                    if (Physics.BoxCast(player.transform.position, player.GetComponent<Collider>().bounds.size, WorldPos - player.transform.position,out Hitpoint, player.transform.rotation, script.viewRadius, script.ObstacleMask))
                    {
                        //Get the interseption point
                        Vector2Int point = World2Indices(Hitpoint.point);
                        //Set the point in the index to unwalkable
                        try
                        {
                            Map[point.x, point.y] = 0.0f;
                        }
                        catch (System.IndexOutOfRangeException ex)
                        {
                            Debug.Log(point);
                        }
                        //Set so the mesh is updated
                        status = true;
                    }
                    else
                    {
                        //Make a box check in is node 
                        if(Physics.CheckBox(WorldPos,new Vector3(grid.nodeRadius,0.001f,grid.nodeRadius),Quaternion.identity,script.ObstacleMask ))
                        {
                            Map[x, y] = 0.0f;
                            status = true;
                        }
                    }
                }
            }
        }
    }

    //Initialize the maps
    void InitArray()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Map[x, y] = 1.0f;
            }
        }
    }
    //Update the grid in the pathfinding algoritm.
    void UpdateGrid()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if(Map[x,y]>0.5)
                {
                    grid.getNode(x, y).SetWalkable(true);
                }
                else
                {
                    grid.getNode(x, y).SetWalkable(false);
                }
                
            }
        }
    }
    //Update Influence map
    void ShowUpdate(float[,] map)
    {
        //This code is from a unity forum thread.
        //Create new texture
        Texture2D texture = new Texture2D((int)size.x, (int)size.y);
        //Creates a new sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, (int)size.x, (int)size.y), Vector2.zero);
        //Give the new sprite to the image
        Im.sprite = sprite;

        //Loop through all pixels
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++) //Goes through each pixel
            {
                Color pixelColour = new Color(map[x, y], map[x, y], map[x, y], 1);
                texture.SetPixel(x, y, pixelColour);
            }
        }
        texture.Apply();
    }

    //Return the filtered map
    public float[,] GetMap()
    {
        return Map;
    }

    //Make World coordinates into normalized coordinates
    public Vector2 World2Normal(Vector3 pos)
    {
        Vector2 NormalPos;
        NormalPos.x = (pos.x + groundSize / 2.0f) / groundSize;
        NormalPos.y = (pos.z + groundSize / 2.0f) / groundSize;
        return NormalPos;
    }
    //Make indicies to World Coordinates
    public Vector3 Indices2World(int x, int y)
    {
        Vector2 pos;
        pos.x = ((float)x / size.x) * groundSize - groundSize / 2.0f+grid.nodeRadius;
        pos.y = ((float)y / size.y) * groundSize - groundSize / 2.0f+grid.nodeRadius;
        return new Vector3(pos.x, player.transform.position.y, pos.y);
    }
    //Make World Coordinates into Incides
    public Vector2Int World2Indices(Vector3 pos)
    {
        Vector2Int Ind = Vector2Int.zero;
        Vector2 Nor = World2Normal(pos);
        Ind.x = (int)(Nor.x * (size.x));
        Ind.y = (int)(Nor.y * (size.y));
        return Ind;
    }
}
