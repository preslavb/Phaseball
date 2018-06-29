using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;

// For tracking game duration
public class TimerScript : MonoBehaviour
{ 
	public float matchLengthMinutes = 5;
	public float matchLengthSeconds = 0;
	float timer;

	string minutes;
	string seconds;

	public Text timerDisplay;

	// References
	public GameObject reset;
	public Button resetButton;
	public Text startMatchText;
	public Text endMatchText;

	// Singleton
	public static TimerScript Instance;

	private static bool matchStarted;

	// Run the countdown whenever match started changes to false (used for countdown after a goal)
	public static bool MatchStarted
	{
		get
		{
			return matchStarted;
		}

		set
		{
			matchStarted = value;
			if (!matchStarted)
			{
				Instance.ResetCountdown();
				CameraController.hasScored = false;
			}
				
			Platformer2DUserControl.controllable = value;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	// Initialization of the timer 
	void Start ()
	{
		Time.timeScale = 1;
		timer = (matchLengthMinutes * 60)+ matchLengthSeconds;

		minutes = Mathf.Floor(timer / 60).ToString("00");
		seconds = Mathf.Floor(timer % 60).ToString("00");

		timerDisplay.text = minutes +" : "+ seconds;
		MatchStarted = false;

		StartCoroutine(StartMatchCoroutine());

		reset.SetActive (false);
	}
	
	// Each frame the delta time is subtracted from the timer and when it is less than or equal to 0 the game ends
	void Update ()
	{
		if (MatchStarted)
		{
			timer -= Time.deltaTime;
			minutes = Mathf.Floor(timer / 60).ToString("00");
			seconds = Mathf.Floor(timer % 60).ToString("00");

			if (timer <= 0)
			{
				timer = 0;

				// Check which team won
				if (GameObject.Find("BlueGoal").GetComponent<GoalScript>().Goals > GameObject.Find("RedGoal").GetComponent<GoalScript>().Goals)
				{
					timerDisplay.text = "Warm Team Won!";
					endMatchText.text = "Warm Team Won!";
				}

				else if (GameObject.Find("BlueGoal").GetComponent<GoalScript>().Goals < GameObject.Find("RedGoal").GetComponent<GoalScript>().Goals)
				{
					timerDisplay.text = "Cool Team Won!";
					endMatchText.text = "Cool Team Won!";
				}

				else
				{
					timerDisplay.text = "Match was a draw";
					endMatchText.text = "Match was a draw";
				}

				// Activate the buttons for reset
				reset.SetActive(true);
				resetButton.Select();
				resetButton.OnSelect(null);
				Time.timeScale = 0;
			}

			else
			{
				timerDisplay.text = minutes + " : " + seconds;
			}
		}
	}

	public void ResetGame()
	{
		// Application.LoadLevel(Application.loadedLevel);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ResetCountdown()
	{
		StartCoroutine(StartMatchCoroutine());
	}

	// Coroutine for the countdown
	private IEnumerator StartMatchCoroutine()
	{
		byte matchCountdown = 3;
		startMatchText.gameObject.SetActive(true);

		AudioClip countdownTick = Resources.Load<AudioClip>("Sound/CountdownTick");

		while (matchCountdown > 0)
		{
			startMatchText.text = matchCountdown.ToString();
			GetComponent<AudioSource>().PlayOneShot(countdownTick);
			yield return new WaitForSeconds(1);
			matchCountdown--;
		}
		BoostRespawn.Instance.SpawnBoost();

		startMatchText.gameObject.SetActive(false);
		MatchStarted = true;
	}
}
