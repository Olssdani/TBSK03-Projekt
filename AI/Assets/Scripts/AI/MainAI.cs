using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAI : MonoBehaviour {

    enum State { Rest, Search, Search_Area, Hunt, Random};
    int Current;
    TargetDetector fow;
    // Use this for initialization
    void Start ()
    {
        Current = 4;
        fow = GetComponent<TargetDetector>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (fow.FindTarget())
        {
            Current = (int)State.Hunt;
        }
        else
        {
            Current = (int)State.Rest;
        }
        //Check which state the ai is in
		if(Current == (int)State.Rest)
        {
            Debug.Log(State.Rest);
            GetComponent<Rest>().run();
        }else if (Current == (int)State.Search)
        {
            Debug.Log(State.Search);
           
        }
        else if (Current == (int)State.Search_Area)
        {
            Debug.Log(State.Search_Area);
        }
        else if (Current == (int)State.Hunt)
        {
            Debug.Log(State.Hunt);
        }
        else if (Current == (int)State.Random)
        {
            Debug.Log(State.Random);
            if (GetComponent<Move_Random>().run())
            {
                Current = (int)State.Rest;
            }

        }
      
    }
}
