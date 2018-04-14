using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {
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
	
	// Update is called once per frame
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
