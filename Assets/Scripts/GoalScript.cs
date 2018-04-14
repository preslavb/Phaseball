using System.Collections;
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

	public GameObject explosion;

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
			StartCoroutine(GoalCoroutine(collision));
		}
	}

	private IEnumerator GoalCoroutine(Collider2D collision)
	{
		Goals++;
		GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/Score"));
		GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/ScoreExplosion"));
		collision.gameObject.transform.parent.GetComponent<BallScript>().Destroy();
		CameraController.hasScored = true;
		CameraController.ballCamera.Follow = CameraController.lastHandledBy.transform;
		//CameraController.ballCamera.transform.SetParent(CameraController.lastHandledBy.transform);
		explosion.SetActive(true);

		yield return new WaitForSeconds(3f);

		CameraController.hasScored = false;
		explosion.SetActive(false);

		foreach (PlatformerCharacter2D character in playerScripts)
		{
			character.ResetPosition();
		}

		TimerScript.MatchStarted = false;

		GameObject newBall = Instantiate(ballPrefab);
		newBall.transform.position = new Vector2(-0.2f, -0.3f);
		newBall.name = "Ball";
	}
}
