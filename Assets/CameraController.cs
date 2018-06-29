using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {

	// Flag for shakey cam
	public static bool hasScored = false;

	public static CinemachineVirtualCamera gameplayCamera;
	public static CinemachineVirtualCamera ballCamera;

	internal static GameObject lastHandledBy;

	// Use this for initialization
	void Start () {

		ballCamera = GameObject.Find("BallCamera").GetComponent<CinemachineVirtualCamera>();
		gameplayCamera = GameObject.Find("GameplayCamera").GetComponent<CinemachineVirtualCamera>();

		ballCamera.gameObject.SetActive(false);
		gameplayCamera.gameObject.SetActive(true);
	}
	
	// Shakey cam or no?
	void Update () {
		if (hasScored)
		{
			gameplayCamera.gameObject.SetActive(false);
			ballCamera.gameObject.SetActive(true);
		}

		else
		{
			gameplayCamera.gameObject.SetActive(true);
			ballCamera.gameObject.SetActive(false);
		}
	}
}
