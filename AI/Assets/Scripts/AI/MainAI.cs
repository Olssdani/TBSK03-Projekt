using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAI : MonoBehaviour {

    enum State { Rest, Search, Search_Area, Hunt, Random};
    int Current;
    TargetDetector fow;
    Movement mov;
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
        if (fow.FindTarget())
        {    
            //Current = (int)State.Hunt;
        }
        else
        {
            Current = (int)State.Search;
        }
  
        //Check which state the ai is in
		if(Current == (int)State.Rest)
        {
            Debug.Log((int)State.Rest);
        }else if (Current == (int)State.Search)
        {
            Debug.Log((int)State.Search);
            GetComponent<Search>().run();
        }
        else if (Current == (int)State.Search_Area)
        {
            Debug.Log((int)State.Search_Area);
        }
        else if (Current == (int)State.Hunt)
        {
            Debug.Log((int)State.Hunt);
            mov.GotoPos(fow.VisableTargets[0]);
        }
        else if (Current == (int)State.Random)
        {
            Debug.Log((int)State.Random);
            if (GetComponent<Move_Random>().run())
            {
                Current = (int)State.Search;
            }   
        }  
    }
}
