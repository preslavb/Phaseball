using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {


    public Text Scoreboard;
    public GameObject ball;
    private int scoreText;
    
    

    private int player1Score = 0;
    private int player2Score = 0;

	// Use this for initialization
	void Start () {

        Scoreboard.text = "";
        ball = GameObject.Find("Ball");
        

		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (ball.gameObject.tag == "Goal") 
        {
            player1Score++;
        }

        if (ball.gameObject.tag == "Goal1")
        {
            player2Score++;
        }

        Debug.Log("Collision Detected");

        Scoreboard.text = player1Score.ToString() + " - " + player2Score.ToString();
        print(player1Score + " - " + player2Score);

        Scoreboard.text = "" + scoreText;
    }

    void OnTriggerEnter(Collider coll)
    {
        scoreText = scoreText + 1;
    }

    
}
