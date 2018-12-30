/*
    This is the state controller

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAI : MonoBehaviour {

    //Vairables
    enum State { Search, Search_Area, Hunt, Hunt_Lost};
    int Current;
    TargetDetector fow;
    Movement mov;
    float delta;
    public GameObject goal;
    Vector3 SearchPos = Vector3.zero;
   
    // Use this for initialization
    void Start ()
    {
        Current = (int)State.Search;
        fow = GetComponent<TargetDetector>();
        mov = GetComponent<Movement>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //If the target is inside the view then go to state hunt.
        if (fow.FindTarget())
        {    
            Current = (int)State.Hunt;
            delta = 0.0f;
        }
        //if the target is lost go to state Hunt Lost
        else if(Current == (int)State.Hunt)
        {
            Current = (int)State.Hunt_Lost;
        }
        
        //Display the state
        DisplayCurrentState();

        //Check which state the ai is in
        if (Current == (int)State.Search)
        {
            if(GetComponent<Search>().run())
            {
                Current = (int)State.Search_Area;
            }
        }
        else if (Current == (int)State.Search_Area)
        {
            if(GetComponent<Search_Area>().run())
            {
                Current = (int)State.Search;
            }
        }
        else if (Current == (int)State.Hunt)
        {      
            mov.GotoPos(fow.VisableTargets[0]);
        }
        else if (Current == (int)State.Hunt_Lost)
        {
            //Get the targets position
            Vector3 HuntPos = goal.transform.position;
           
            //Set the last seen position to the search position
            if (delta <0.0001f)
            {
                SearchPos = HuntPos;
            }
            //Add the time
            delta = delta + Time.deltaTime;
           
            //If the time is over a value stop hunt and start seach
            if (delta>8.0f)
            {
                delta = 0.0f;
                Current = (int)State.Search;
            }
           
            //Give a new search postion with a distortion
            if (mov.GotoPos(SearchPos))
            {
                float distortion = delta;
                SearchPos = new Vector3(Random.Range(HuntPos.x - distortion, HuntPos.x + distortion), HuntPos.y, Random.Range(HuntPos.z - distortion, HuntPos.z + distortion));
            }
            

        }
    }
    //Display the state the agent is in
    void DisplayCurrentState()
    {
        
        if (Current == (int)State.Search)
        {
            Debug.Log("Search");
        }
        else if (Current == (int)State.Search_Area)
        {
            Debug.Log("Search Area");
        }
        else if (Current == (int)State.Hunt)
        {
            Debug.Log("Hunt");
        }
        else if (Current == (int)State.Hunt_Lost)
        {
            Debug.Log("Hunt_Lost");
        }
    }

    //Share the influence maps if two agents is close
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            gameObject.GetComponent<InflunceMap>().Share(other.gameObject.GetComponent<InflunceMap>().GetUnfiltred());
        }
    }
}
