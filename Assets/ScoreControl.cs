using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreControl : MonoBehaviour
{

    public GameObject Goal;
    public GameObject Goal1;
    
    private int scoreText1;
    private int scoreText2;


    public int player1Score = 0;
    public int player2Score = 0;


    // Use this for initialization
    void Start()
    {
        Goal = GameObject.Find("Goal");
        Goal1 = GameObject.Find("Goal1");
    }

    // Update is called once per frame
    void Update()
    {
        int playerNumberWhoScored = 0;

        Debug.Log("Collision Detected");

        scoreText1 = scoreText1 + 1;
        scoreText2 = scoreText2 + 1;
        //scoreText1++;
        //scoreText2++;


        //if (Ball.gameObject.tag == "Goal1")
        // {
        //     transform.position = gameObject.transform.position;
        // }



        //if (Ball.gameObject.tag == "Goal")
        //{
        //   transform.position = gameObject.transform.position;
        // }
        //}


    }

    void OnTriggerEnter2D(Collision2D coll)
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
    }
}





