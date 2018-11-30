using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour {

    InflunceMap map;
    int size;
    const int kernelSize =3;
    int loopSize;
    bool search_new = true;
    Vector3 MoveToPos = Vector3.zero;
    // Use this for initialization
    void Start () {
        map = GetComponent<InflunceMap>();
        size = map.GetSize();
        loopSize = size / kernelSize;
    }

    public bool run()
    {

        if (search_new)
        {
            MoveToPos = GetPosition();
            search_new = false;
        }
        if (gameObject.GetComponent<Movement>().GotoPos(MoveToPos))
        {
            
            search_new = true;
            return true;
        }
        return false;
    }




    Vector3 GetPosition()
    {
        //Loop thorugh the whole influnce map, discard the outer values
        float[,] CurrentMap = map.GetMap();
        List<Vector3> positions = new List<Vector3>();
        Vector3 pos = transform.position;
        float min = 10;

        for (int x = 1; x < size; x = x + kernelSize)
        {
            for (int y = 1; y < size; y = y + kernelSize)
            {
                float sum =
                    CurrentMap[x - 1, y - 1] + CurrentMap[x, y - 1] + CurrentMap[x + 1, y - 1] +
                    CurrentMap[x - 1, y] + CurrentMap[x, y] + CurrentMap[x + 1, y] +
                    CurrentMap[x - 1, y + 1] + CurrentMap[x, y + 1] + CurrentMap[x + 1, y + 1];

                if (sum <min)
                {
                    min = sum;
                    positions.Clear();
                    positions.Add(map.Indices2World(x, y));
                }else if(Mathf.Abs(sum-min)< 0.05)
                {
                    positions.Add(map.Indices2World(x, y));
                }
            }
        }
        pos = positions[Mathf.RoundToInt(Random.Range(0, positions.Count))]; ;

        return pos;
    }
}
