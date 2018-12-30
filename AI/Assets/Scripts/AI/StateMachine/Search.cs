/*
    This is the state where the agent use the influence map to search a new position

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour {
    //Variables
    InflunceMap map;
    int size;
    const int kernelSize =3;
    bool search_new = true;
    Vector3 MoveToPos = Vector3.zero;
   
    // Use this for initialization
    void Start () {
        map = GetComponent<InflunceMap>();
        size = map.GetSize();
    }

    //Run function
    public bool run()
    {
        //If a new position should be calulated
        if (search_new)
        {
            MoveToPos = GetPosition();
            search_new = false;
        }
        //Move agent closer to the target and if it return true we are there
        if (gameObject.GetComponent<Movement>().GotoPos(MoveToPos))
        {
            search_new = true;
            return true;
        }
        return false;
    }



    //Calculates a new position from the influence map
    Vector3 GetPosition()
    {
        float[,] CurrentMap = map.GetMap();
        List<Vector3> positions = new List<Vector3>();
        Vector3 pos = transform.position;
        float min = 10;
        //Loop through the map
        for (int x = 1; x < size; x = x + kernelSize)
        {
            for (int y = 1; y < size; y = y + kernelSize)
            {
                //Sum the values inside the kernel.
                float sum =
                    CurrentMap[x - 1, y - 1] + CurrentMap[x, y - 1] + CurrentMap[x + 1, y - 1] +
                    CurrentMap[x - 1, y] + CurrentMap[x, y] + CurrentMap[x + 1, y] +
                    CurrentMap[x - 1, y + 1] + CurrentMap[x, y + 1] + CurrentMap[x + 1, y + 1];

                //Min function
                if (sum <min)
                {
                    min = sum;
                    positions.Clear();
                    positions.Add(map.Indices2World(x, y));
                }
                // Add if the value is close enough
                else if(Mathf.Abs(sum-min)< 0.05)
                {

                    positions.Add(map.Indices2World(x, y));
                }
            }
        }
        //Random one of the five first positions in the list
        pos = positions[Mathf.RoundToInt(Mathf.Min(Random.Range(0, positions.Count),5.0f))]; ;

        return pos;
    }
}
