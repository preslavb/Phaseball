using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class TimerScript : MonoBehaviour
{ 
	public float matchLengthMinutes = 5;
	public float matchLengthSeconds = 0;
	float timer;

	string minutes;
	string seconds;

    public Text timerDisplay;

	public GameObject reset;
	public Button resetButton; 

	// Initialization of the timer 
	void Start ()
    {
		Time.timeScale = 1;
		timer = (matchLengthMinutes * 60)+ matchLengthSeconds;

		minutes = (timer / 60).ToString("0");
		seconds = (timer % 60).ToString("00");

		timerDisplay.text = minutes +":"+ seconds;

		reset.SetActive (false);
	}
	
	// Each frame the delta time is subtracted from the timer and when it is less than or equal to 0 the EndMenu scene is loaded
	void Update ()
    {


        timer -= Time.deltaTime;
		minutes = (timer / 60).ToString("0");
		seconds = (timer % 60).ToString("00");

		if (timer <= 0) 
		{
			timer = 0;
			timerDisplay.text = "Match Over";

			reset.SetActive(true);
			resetButton.Select();
			resetButton.OnSelect (null);
			//EventSystem.current.SetSelectedGameObject(reset);
			Time.timeScale = 0;


		} 
		else 
		{
			timerDisplay.text = minutes +":"+ seconds;
		} 
    }

	public void ResetGame()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
