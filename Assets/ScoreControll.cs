﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreControll : MonoBehaviour {


	public GameObject Goal;
	public GameObject Goal1;
    public GameObject Ball;
	private int scoreText1;
	private int scoreText2;


	public int player1Score = 0;
	public int player2Score = 0;

	// Use this for initialization
	void Start () {

	  
		
		Goal = GameObject.Find("Goal");
		Goal1 = GameObject.Find("Goal1");
		GameObject.Find("Ball");
		

	}
	
	// Update is called once per frame
	void Update ()
	{
		//int playerNumberWhoScored = 0;

	  
		// if (Goal.gameObject.tag == "Ball") 
	   // {
		// scoreText1++;
			
	   // }

	   // if (Goal1.gameObject.tag == "Ball")
	  //  {
		//  scoreText2++;
			
	   // }

	  



		Debug.Log("Collision Detected");


        // print(player1Score + " - " + player2Score);


        if (Ball.gameObject.tag == "Goal1")
        {
            transform.position = gameObject.transform.position;
        }



        if (Ball.gameObject.tag == "Goal")
        {
            transform.position = gameObject.transform.position;
        }
    }

   

   


	void OnTriggerEnter2D(Collider coll)
	{
		if(Ball.gameObject.tag == "Goal1")
		{
			scoreText1 = scoreText1 + 1;
		}

		if(Ball.gameObject.tag == "Goal")
		{
			scoreText2 = scoreText2 + 1;
		}

		//scoreText1 = scoreText1 + 1;
		//scoreText2 = scoreText2 + 1;
	}

	
}
