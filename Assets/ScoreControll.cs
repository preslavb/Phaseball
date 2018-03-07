using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreControll : MonoBehaviour {


	public GameObject Goal;
	public GameObject Goal1;
	private int scoreText1;
	private int scoreText2;


	public int player1Score = 0;
	public int player2Score = 0;

	// Use this for initialization
	void Start () {

	  
		
		Goal = GameObject.Find("Goal");
		Goal1 = GameObject.Find("Goal1");
		

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

   

   


	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Goal")
		{
			if (coll.gameObject.name == "Goal")
			{
				scoreText1 = scoreText1 + 1;
			}

			else
			{
				scoreText2 = scoreText2 + 1;
			}
		}
		

		scoreText1 = scoreText1 + 1;
		scoreText2 = scoreText2 + 1;
	}

	
}
