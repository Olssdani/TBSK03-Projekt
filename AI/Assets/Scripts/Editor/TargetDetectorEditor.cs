using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (TargetDetector))]
public class TargetDetectorEditor : Editor {
        
    void OnSceneGUI()
    {
        TargetDetector fow = (TargetDetector)target;
        Handles.color = Color.white;
       
        Vector3 viewAngleL = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleR = fow.DirFromAngle(fow.viewAngle / 2, false);
        Handles.DrawWireArc(fow.transform.position, Vector3.up, viewAngleL, fow.viewAngle, fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleL * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleR * fow.viewRadius);
        Handles.color = Color.red;
        foreach (Transform t in fow.VisableTargets)
        {
            Handles.DrawLine(fow.transform.position, t.position);
        }
        

    }

}
