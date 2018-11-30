using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search_Area : MonoBehaviour
{
    bool Find_New_Position = true;
    Vector3 MoveToPos = Vector3.zero;
    InflunceMap map;
    Movement mov;
    Vector2 Search_Pos = Vector2.zero;
    bool RotationDone = false;
    int Case_LookAround;
    int Case;
    Vector3 left;
    Vector3 right;
    Vector3 forward;


    void Start()
    {
        map = GetComponent<InflunceMap>();
        mov = GetComponent<Movement>();
        Case_LookAround = 0;
        Case = 0;
    }

    public bool run()
    {
        Vector2 IndPos = map.World2Indices(transform.position);
        switch (Case)
        {
            case 0:
                if (LookToDirection(IndPos))
                {
                    Case++;
                }
                break;
            case 1:
                if (Find_New_Position)
                {
                    MoveToPos = GetPosition(IndPos);
                    Debug.Log(MoveToPos);
                    Find_New_Position = false;
                }
                if (mov.GotoPos(MoveToPos))
                {
                    Case++;
                }
                break;
            case 2:
                if (LookToDirection(IndPos))
                {
                    Case++;
                }
                break;
            default:
                Case = 0;
                Find_New_Position = true;
                return true;
        }

        return false;
    }






    bool LookToDirection(Vector2 Indic)
    {
        if (!RotationDone)
        {
            right = transform.position + Vector3.Cross(transform.up, transform.forward);
            left = transform.position + Vector3.Cross(transform.forward, transform.up);
            forward = transform.position + transform.forward;
            RotationDone = true;
        }

        switch (Case_LookAround)
        {
            case 0:
                if (mov.LookAt(left))
                {
                    Case_LookAround++;
                }
                break;
            case 1:
                if (mov.LookAt(forward))
                {
                    Case_LookAround++;
                }
                break;
            case 2:
                if (mov.LookAt(right))
                {
                    Case_LookAround++;
                }
                break;
            case 3:
                if (mov.LookAt(forward))
                {
                    Case_LookAround++;
                }
                break;
            default:
                Case_LookAround = 0;
                RotationDone = false;
                return true;
        }
        return false;
    }



    Vector3 GetPosition(Vector2 ind)
    {
        float minX = Mathf.Max(1, ind.x - 4.0f);
        float maxX = Mathf.Min(map.GetSize() - 1, ind.x + 4.0f);
        float minZ = Mathf.Max(1, ind.y - 4.0f);
        float maxZ = Mathf.Min(map.GetSize() - 1, ind.y + 4.0f);

        int x = Mathf.RoundToInt(Random.Range(minX, maxX));
        int z = Mathf.RoundToInt(Random.Range(minZ, maxZ));
        Vector2 temp = map.Indices2World(x, z);
        return new Vector3(temp.x, transform.position.y, temp.y) ;
    }
}


/* Vector2 FindMinMapPos(Vector2 ind)
 {
     Debug.Log(ind);
     int minX = (int)Mathf.Max(1, ind.x - 5.0f);
     int maxX = (int)Mathf.Min(map.GetSize()-1, ind.x + 5.0f);
     int minY = (int)Mathf.Max(1, ind.y - 5.0f);
     int maxY = (int)Mathf.Min(map.GetSize()-1, ind.y + 5.0f);


     float min = 2.0f;
     Vector2 pos = Vector2.zero;
     float[,] CurrentMap = map.GetMap();
     Debug.Log("Min x " + minX + " Max X " + maxX + " Min y " + minY + " Max Y " + maxY);
     for (int x = minX; x <= maxX; ++x)
     {
         for (int y = minY; y <= maxY; ++y)
         {

             if (CurrentMap[x, y] < min)
             {
                 min = CurrentMap[x, y];
                 pos.x = x;
                 pos.y = y;
             }
         }
     }

     Debug.Log(pos);
     //int x = (int)Random.Range(Mathf.Max(0, ind.x - 5), Mathf.Max(map.GetSize(), ind.x + 5));
     //int y = (int)Random.Range(Mathf.Max(0, ind.y - 5), Mathf.Max(map.GetSize(), ind.y + 5));
     return pos;
 }*/
