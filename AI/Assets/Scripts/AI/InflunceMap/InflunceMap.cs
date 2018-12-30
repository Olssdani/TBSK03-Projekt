/*
This script updates the influence map and also displays them in the editor
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InflunceMap : MonoBehaviour {
    //Variables
    public Image Im;
    //Must be able to divided by 3!!!!!!!!!!!!!!!
    public int size =30;
    //Maps
    private float[,] Unfiltered;
    private float[,] Filtered;
    GameObject player;
    public float decreasetime;
    private float[,] temp;
    public int NumberOfLowPass;
    public Collider col;
    float groundSize = 0;

    //initialization
    void Awake()
    {
        // Maps
        Unfiltered = new float[size, size];
        Filtered = new float[size, size];
        temp = new float[size, size];
        InitArray();
        groundSize = col.bounds.size.x ;;
        player = this.gameObject;
    }
	// Update is called once per frame
	void Update ()
    {
        //update the maps
        UpdateMap();

        //Set the values of the unfiltred map to the temporary map
        AssignTo(Unfiltered, temp);

        //Lowpass filter the image into the Filtred image
        if(NumberOfLowPass == 0)
        {
            AssignTo(temp, Filtered);
        }
        else
        {
            //Ping pong 
            for (int i = 0; i < NumberOfLowPass; ++i)
            {
                if (i % 2 == 0)
                {
                    LowPass(temp, Filtered);
                }
                else
                {
                    LowPass(Filtered, temp);
                }
            }
        }
        

        //Update the image in the editor
        ShowUpdate(Filtered);    
    }
    //Update the Influence map
    void UpdateMap(){
        //Loop throught the unfiltered map and decrease its value depending on the time since last frame.
        for (int y = 0; y < size; y++){
            for (int x = 0; x < size; x++){
                if(Unfiltered[x, y] > 0.0f){
                    Unfiltered[x, y] -= 1.0f*(Time.deltaTime/ decreasetime);
                } else{
                    Unfiltered[x, y] = 0.0f;
                }    
            }
        }
        //Get the position of the player i world coordinates and Normal coordinates
        Vector3 pos = player.transform.position;
        Vector2 NormalPos = World2Normal(pos);
        //Set the position of the player in the influence map to 1.0.
        Unfiltered[(int)(NormalPos.x * size), (int)(NormalPos.y * size)] = 1.0f;
        TargetDetector script = GetComponent<TargetDetector>();
        //Loop through all positions in the influence map and 
        //check if they are inside the view radius and view angle of the agent
        for (int y = 0; y < size; y++){
            for (int x = 0; x < size; x++){
                Vector3 WorldPos = Indices2World(x, y);
                if (Vector3.Distance(transform.position, WorldPos)<script.viewRadius){
                    if(Vector3.Angle(transform.forward,WorldPos-transform.position )<script.viewAngle/2){
                        //If the position is inside the agents view then a simple raycast is made to check if any obstacle is in the way
                        if(!Physics.Raycast(transform.position, WorldPos - transform.position, 
                            Vector3.Distance(transform.position, WorldPos), script.ObstacleMask)){
                            //if position is visable then the position will get a value greater than 0
                            Unfiltered[x, y] = Mathf.Min(Unfiltered[x, y]+1.0f - 
                                Mathf.Pow(Vector3.Distance(transform.position, WorldPos) / script.viewRadius,2), 1.0f);
                        }
                    }
                }
            }
        }
    }

    //Set all maps value to 0
    void InitArray()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Filtered[x, y] = 0.0f;
                Unfiltered[x, y] = 0.0f;
                temp[x, y] = 0.0f;
            }
        }
    }

    //Lowpass filter the maps
    void LowPass(float[,] inmap, float[,] outmap)
    {
        for (int y = 1; y < size-1; y++)
        {
            for (int x = 1; x < size-1; x++)
            {
                outmap[x, y] = (float)(1.0/2.0)* inmap[x, y] + (float)(1.0/16.0)*(inmap[x - 1, y - 1] 
                + inmap[x, y - 1] + inmap[x + 1, y - 1] + inmap[x - 1, y] + inmap[x + 1, y] 
                + inmap[x - 1, y + 1] + inmap[x, y + 1] + inmap[x + 1, y + 1]);       
                
            }
        }

    }

    //Set one maps value to the other
    void AssignTo(float[,] Inmap, float[,] outmap)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                outmap[x, y] = Inmap[x, y];
            }
        }
    }
    //Update Influence map
    void ShowUpdate(float[,] map)
    {
        //This code is from a unity forum thread, can't find the link anymore...
        //Create new texture
        Texture2D texture = new Texture2D(size, size);
        //Creates a new sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);
        //Give the new sprite to the image
        Im.sprite = sprite;
        //Loop through the size of the texture
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++) //Goes through each pixel
            {
                //Set the pixelcolor
                Color pixelColour = new Color(map[x,y], map[x, y], map[x, y], 1); 
                texture.SetPixel(x, y, pixelColour);
            }
        }
        texture.Apply();
    }
    //Get Size of map
    public int GetSize()
    {
        return size;
    }

    //Return the filtered map
    public float[,] GetMap()
    {
        return Filtered;
    }
    //return the unfiltred map
    public float[,] GetUnfiltred()
    {
        return Unfiltered;
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
        pos.x = ((float)x / size) * groundSize - groundSize / 2.0f;
        pos.y = ((float)y / size) * groundSize - groundSize / 2.0f;
        return new Vector3(pos.x, player.transform.position.y, pos.y);
    }
    //Make World Coordinates into Incides
    public Vector2 World2Indices(Vector3 pos )
    {
        Vector2 Ind;
        Vector2 Nor = World2Normal(pos);  
        Ind.x = (int)(Nor.x * size);
        Ind.y = (int)(Nor.y * size);
        return Ind;
    }

    //Share the influence map between two agents
    public void Share(float[,] map)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                //The map with the highest value gives that value to the other.
                if(map[x,y]>Unfiltered[x,y])
                {

                    Unfiltered[x, y] = map[x, y];
                }
                else
                {
                    map[x, y] = Unfiltered[x, y];
                }
            }
        }
    }
}
