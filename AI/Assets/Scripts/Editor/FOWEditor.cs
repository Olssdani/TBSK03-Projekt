/*
 Editor script taken from Sebastian Lagues that display the Field of view 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FOW))]
public class FOWEditor : Editor {
    void OnSceneGUI()
    {
        FOW fow = (FOW)target;
        Handles.color = Color.white;

        Vector3 viewAngleL = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleR = fow.DirFromAngle(fow.viewAngle / 2, false);
        Handles.DrawWireArc(fow.transform.position, Vector3.up, viewAngleL, fow.viewAngle, fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleL * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleR * fow.viewRadius);
        Handles.color = Color.red;
        foreach(Vector3 r in fow.rays)
        {
            Handles.DrawLine(fow.transform.position, fow.transform.position + r);
        }



    }

}
