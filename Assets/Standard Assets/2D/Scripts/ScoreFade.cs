using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreFade : MonoBehaviour 
{
	public CanvasGroup uiScore;

	// Use this for initialization
	void Start ()
	{
		uiScore.alpha = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Fire1"))
		{
			StartCoroutine (Fade (uiScore, uiScore.alpha, 1));
		}
		if (Input.GetButtonUp ("Fire1")) 
		{
			StartCoroutine (Fade (uiScore, uiScore.alpha, 0));
		}
	}
	IEnumerator Fade(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
	{
		float timeStarted = Time.time;
		float timeSinceStart = Time.time - timeStarted;
		float percentCompleat = timeSinceStart/lerpTime;

		while (true) 
		{
			timeSinceStart = Time.time - timeStarted;
			percentCompleat = timeSinceStart/lerpTime;

			float CurrentValue = Mathf.Lerp (start, end, percentCompleat);

			cg.alpha = CurrentValue;

			if (percentCompleat >= 1) 
			{
				break;
			}
			yield return new WaitForEndOfFrame ();
		}
	}

}
