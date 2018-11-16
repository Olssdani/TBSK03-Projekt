﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public GameObject player;
    Transform Ptransform;
    float offset = 0.0f;
    // Use this for initialization
    void Start () {
        Ptransform = player.GetComponent<Transform>();
        offset = transform.position.y - Ptransform.position.y;
        transform.position = new Vector3(Ptransform.position.x, Ptransform.position.y + offset, Ptransform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Ptransform.position.x, Ptransform.position.y + offset, Ptransform.position.z);
    }
}
