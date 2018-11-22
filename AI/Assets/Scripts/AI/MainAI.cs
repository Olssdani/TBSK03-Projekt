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
        Current = 3;
        fow = GetComponent<TargetDetector>();
        mov = GetComponent<Movement>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (fow.FindTarget())
        {
          
            Current = (int)State.Hunt;
        }
  
        //Check which state the ai is in
		if(Current == (int)State.Rest)
        {

        }else if (Current == (int)State.Search)
        {
           
        }
        else if (Current == (int)State.Search_Area)
        {

        }
        else if (Current == (int)State.Hunt)
        {
            Debug.Log("Hej");
            mov.GotoPos(new Vector3(0, 0, 0));
        }
        else if (Current == (int)State.Random)
        {

        }
      
    }
}
