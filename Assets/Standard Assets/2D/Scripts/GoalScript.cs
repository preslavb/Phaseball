using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityStandardAssets._2D;

public enum Team {
	Blue,
	Red
}

public class GoalScript : MonoBehaviour {
	public Text textForScore;

	public Team teamBelongingTo;

	public UnityAction onScore;

	public PlatformerCharacter2D[] playerScripts;

	public GameObject ballPrefab;

	[SerializeField] private int goals = 0;

	public int Goals {
		get
		{
			return goals;
		}

		set
		{
			goals = value;
			textForScore.text = goals.ToString();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Ball")
		{
			Goals++;
			GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/Score"));

			foreach (PlatformerCharacter2D character in playerScripts)
			{
				character.ResetPosition();
			}

			collision.gameObject.transform.parent.GetComponent<BallScript>().Destroy();
			GameObject newBall = Instantiate(ballPrefab);
			newBall.transform.position = new Vector2(-0.2f, -0.3f);
			newBall.name = "Ball";
		}
	}
}
