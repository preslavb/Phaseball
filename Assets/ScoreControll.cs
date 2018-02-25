using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreControll : MonoBehaviour {


    public GameObject Goal;
    public GameObject Goal1;
    public GameObject ball;
    private int scoreText1;
    private int scoreText2;

    //


    public int player1Score = 0;
    public int player2Score = 0;

	// Use this for initialization
	void Start () {

      
        ball = GameObject.Find("Ball");
        Goal = GameObject.Find("Goal");
        Goal1 = GameObject.Find("Goal1");

        

    }
	
	// Update is called once per frame
	void Update ()
    {
        int playerNumberWhoScored = 0;

        if (ball.gameObject.tag == "Goal1") 
        {
            scoreText1++;
            
        }

        if (ball.gameObject.tag == "Goal")
        {
            scoreText2++;
            
        }

        Debug.Log("Collision Detected");

        
        print(player1Score + " - " + player2Score);

        
    }

   


    void OnTriggerEnter(Collider coll)
    {
        scoreText1 = scoreText1 + 1;
        scoreText2 = scoreText2 + 1;
    }

    
}
