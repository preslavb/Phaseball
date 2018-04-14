using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Bounce"));
	}

	public void Destroy()
	{
		if (audioSource.isPlaying)
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<CircleCollider2D>().enabled = false;
			gameObject.GetComponent<TrailRenderer>().enabled = false;
			gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
			StartCoroutine(DestroyCoroutine());
		}

		else
		{
			Destroy(gameObject);
		}
	}

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
