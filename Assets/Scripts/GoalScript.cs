using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets._2D;

// Enum of teams in a match
public enum Team {
	Blue,
	Red
}

// Script to be attached to the goal objects. Handles scoring.
public class GoalScript : MonoBehaviour {

	// The UI element to display the score in
	public Text textForScore;

	// The team this goal belongs to
	public Team teamBelongingTo;

	// Callback to run when scoring
	public UnityAction onScore;

	// An array of all player scripts currently in play
	public PlatformerCharacter2D[] playerScripts;

	// A reference to the ball prefab
	public GameObject ballPrefab;

	// A reference to the explosion object (runs an explosion animation when activated in the scene)
	public GameObject explosion;

	// The number of goals that have been scored in this goal
	[SerializeField] private int goals = 0;

	// Property for setting goals and updating the dashboard score (should always be used instead of the private field)
	public int Goals {

		// Return the current goal value
		get
		{
			return goals;
		}

		// Set the goals to a new value and update the UI element
		set
		{
			goals = value;
			textForScore.text = goals.ToString();
		}
	}

	// When a ball collision has been detected, that means there has been a goal, so start the goal coroutine
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Ball")
		{
			StartCoroutine(GoalCoroutine(collision));
		}
	}

	// The goal coroutine
	private IEnumerator GoalCoroutine(Collider2D collision)
	{

		// Increment the goals scored
		Goals++;

		// Play the score sound effects
		GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/Score"));
		GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/ScoreExplosion"));

		// Destroy the ball game object
		collision.gameObject.transform.parent.GetComponent<BallScript>().Destroy();

		// Tell the static camera controller that a goal has been scored, and the camera should follow the person who last handled the ball (and therefore scored)
		CameraController.hasScored = true;
		CameraController.ballCamera.Follow = CameraController.lastHandledBy.transform;

		// Activate the explosion effect (which is in the form of a game object)
		explosion.SetActive(true);

		// Keep the shakey cam on for an amount of seconds
		yield return new WaitForSeconds(3f);

		// Disable the shakey cam effect and disable the explosion effect
		CameraController.hasScored = false;
		explosion.SetActive(false);
		
		// Reset the positions of the characters
		foreach (PlatformerCharacter2D character in playerScripts)
		{
			character.ResetPosition();
		}

		// Unset the match started, thereby reactivating the 3, 2, 1 countdown
		TimerScript.MatchStarted = false;
		
		// BUG: currently the arena 1 is misaligned, so the ball needs to be spawned at a bit of an offset
		// Instantiate a new ball and place it in the middle of the arena
		GameObject newBall = Instantiate(ballPrefab);
		newBall.transform.position = new Vector2(-0.2f, -0.3f);

		// Make sure the ball is named appropriately for collision detection (TODO: should be revised)
		newBall.name = "Ball";
	}
}
