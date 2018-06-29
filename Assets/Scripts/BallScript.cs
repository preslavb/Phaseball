using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{	
	// The audio source attached to the Ball gameObject
	private AudioSource audioSource;

	// On awake save a reference to the audio source of the ball
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// When colliding with objects, play an impact sound (only works for layers that the ball is allowed to collide with, which doesn't include palyers)
	private void OnCollisionEnter2D(Collision2D collision)
	{
		audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Bounce"));
	}

	public void Destroy()
	{

		// If the audio for a bounce or a goal is still playing, disable all visuals and physics on the ball, but keep the object active until the sounds have finished
		if (audioSource.isPlaying)
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<CircleCollider2D>().enabled = false;
			gameObject.GetComponent<TrailRenderer>().enabled = false;
			gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;

			// Run the coroutine that waits for the sounds to finish and destroys the object
			StartCoroutine(DestroyCoroutine());
		}

		// Otherwise, immediately destroy the ball object
		else
		{
			Destroy(gameObject);
		}
	}

	// The delayed distruction coroutine, checks every second whether the audio has finished playing, and if so, destroys the game object
	private IEnumerator DestroyCoroutine()
	{
		yield return new WaitForSecondsRealtime(1);

		while (audioSource.isPlaying)
		{
			yield return new WaitForSecondsRealtime(1);
		}

		Destroy(gameObject);
	}
}
