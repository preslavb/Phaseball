using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour 
{
	public float slowFactor = 0.1f;

	public void SlowDownTime()
	{
		Time.timeScale = slowFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
		print ("Slow time script");
	}

	public void NormalTime()
	{
		Time.timeScale = 1;
		//print ("Normal time");
	}
}
