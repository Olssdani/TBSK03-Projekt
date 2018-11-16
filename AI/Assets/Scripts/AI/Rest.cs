using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : MonoBehaviour {

    float delta;
    float Rcounter;
    float Fcounter;
    Movement script;
    int current;
    //Angle for spinn
    float angle;

    // Use this for initialization
    void Start () {
        delta = 0;
        Rcounter = 0.0f;
        Fcounter = 0.0f;
        script = gameObject.GetComponent<Movement>();
        current = 0;
        angle = -1.0f;
    }
	
	// Update is called once per frame
    
    public void run()
    {
        if(current == 0)
        {
            float r = Random.Range(0.0f, 100.0f);
            if (r < 80.0f)
            {
                Debug.Log("current = 0");
                current = 0;
            }else if(r < 90.0f)
            {
                Debug.Log("current = 1");
                current = 1;
            }
            else
            {
                Debug.Log("current = 2");
                current = 2;
            }
        }
        else if(current == 1)
        {
            Wiggle();
        }
        else if(current == 2)
        {
            Spinn();
        }
    }

    void Wiggle()
    {
        delta += Time.deltaTime;
        if (delta > 5.0f)
        {
            if (Rcounter < 0.5f)
            {
                script.Controller(0, 2.0f);

            }
            else if (Rcounter >= 0.5f && Rcounter < 1.5f)
            {
                script.Controller(0, -2.0f);
            }
            else if (Rcounter >= 1.5f && Rcounter < 2.0f)
            {
                script.Controller(0, 2.0f);
            }

            if (Rcounter < 2.0f)
            {
                Rcounter += Time.deltaTime;
            }
            else
            {
                delta = 0.0f;
                Rcounter = 0.0f;
                current = 0;
                script.Controller(0, 0.0f);
            }
        }
    }

    void Spinn()
    {
        if(angle < 0.0f)
        {
            angle = Random.Range(0.0f, 360.0f);
        }
        

        float rotL = script.GetRot().eulerAngles.y - angle;
        float rotR = script.GetRot().eulerAngles.y - angle;


        if (Mathf.Abs(rotL)<0.1f || Mathf.Abs(rotR) < 0.1f)
        {
            angle = -1.0f;
            current = 0;
            script.Controller(0.0f, 0.0f);
            
        }
        else if(Mathf.Abs(rotR)< Mathf.Abs(rotL))
        {
            script.Controller(0.0f, 2.0f);
          
        }
        else
        {
            script.Controller(0.0f, -2.0f);
       
        }

    }


}


