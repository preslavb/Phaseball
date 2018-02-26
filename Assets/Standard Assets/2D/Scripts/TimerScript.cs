using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    //decleration of the float variable and text object used in the script 
    
	public float matchLengthMinutes = 5;
	public float matchLengthSeconds = 0;
	float timer;

	string minutes;
	string seconds;

    public Text timerDisplay;

	// Initialization of the timer 
	void Start ()
    {
		timer = (matchLengthMinutes * 60)+ matchLengthSeconds;

		minutes = (timer / 60).ToString("00");
		seconds = (timer % 60).ToString("00");

		timerDisplay.text = minutes +" : "+ seconds;
	}
	
	// Each frame the delta time is subtracted from the timer and when it is less than or equal to 0 the EndMenu scene is loaded
	void Update ()
    {


        timer -= Time.deltaTime;
		minutes = (timer / 60).ToString("00");
		seconds = (timer % 60).ToString("00");

		if (timer <= 0) {
			timer = 0;
			timerDisplay.text = "Match Over";
			//Application.LoadLevel("EndMenu");
		} else 
		{
			timerDisplay.text = minutes +" : "+ seconds;
		} 
    }
}
