/*
    This is the state where the agent search in a smaller area

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search_Area : MonoBehaviour
{
    //Variables
    bool Find_New_Position = true;
    Vector3 MoveToPos = Vector3.zero;
    InflunceMap map;
    Movement mov;
    bool RotationDone = false;
    int Case_LookAround;
    int Case;
    Vector3 left;
    Vector3 right;
    Vector3 forward;

    //Initialize vairables
    void Start()
    {
        map = GetComponent<InflunceMap>();
        mov = GetComponent<Movement>();
        Case_LookAround = 0;
        Case = 0;
    }

    // Run the state
    public bool run()
    {
        Vector2 IndPos = map.World2Indices(transform.position);
        //Switch between going to a new position and looking around
        switch (Case)
        {
            case 0:
                //Starts looking around
                if (LookToDirection(IndPos))
                {
                    Case++;
                }
                break;
            case 1:
                //Goes to a new position
                if (Find_New_Position)
                {
                    MoveToPos = GetPosition(IndPos);
                    Find_New_Position = false;
                }
                if (mov.GotoPos(MoveToPos))
                {
                    Case++;
                }
                break;
            case 2:
                //Looks around at the new position
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
    //Function to look left and right
    bool LookToDirection(Vector2 Indic)
    {
        //Get the left and right vector.
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


    //Calculate a position in a close proximity to the agent
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
