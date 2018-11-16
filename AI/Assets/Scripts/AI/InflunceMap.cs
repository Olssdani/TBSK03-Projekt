﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InflunceMap : MonoBehaviour {
    public Image Im;
    const int size =30;
    private float[,] Unfiltered = new float[size, size];
    private float[,] Filtered = new float[size, size];
    GameObject player;
    public float decreasetime;
    private float[,] temp = new float[size, size];
    public int NumberOfLowPass;

    // Use this for initialization
    void Start()
    {
        InitArray();
        //player = GetComponent<GameObject>();
        player = this.gameObject;
    }
	// Update is called once per frame
	void Update ()
    {
        //1: Lower the values by time
        //2: Update the current position to the map
        UpdateMap();

        AssignTo(Unfiltered, temp);


        //3: Lowpass filter the image into the Filtred image
        for(int i =0; i< NumberOfLowPass; ++i )
        {
            if(i%2 == 0)
            {
                LowPass(temp, Filtered);
            }
            else
            {
                LowPass(Filtered, temp);
            }
        }
        
        //Update
        ShowUpdate(Filtered);    
    }

    void UpdateMap()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if(Unfiltered[x, y] > 0.0f)
                {
                    Unfiltered[x, y] -= 1.0f*(Time.deltaTime/ decreasetime);
                }
                else
                {
                    Unfiltered[x, y] = 0.0f;
                }    
            }
        }
        Vector3 pos = player.transform.position;
        pos.x = (pos.x + 5.0f) / 10.0f;
        pos.z = (pos.z + 5.0f) / 10.0f;
        Unfiltered[(int)(pos.x * size), (int)(pos.z * size)] = 1.0f;
     
    }


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
        //This code is from a unity forum thread.
        //Create new texture
        Texture2D texture = new Texture2D(size, size);
        //Creates a new sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);
        //Give the new sprite to the image
        Im.sprite = sprite;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++) //Goes through each pixel
            {
                Color pixelColour = new Color(map[x,y], map[x, y], map[x, y], 1); 
                texture.SetPixel(x, y, pixelColour);
            }
        }
        texture.Apply();
    }
}