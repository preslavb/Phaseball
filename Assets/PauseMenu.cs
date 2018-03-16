using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PauseMenu : MonoBehaviour {
	public Platformer2DUserControl playerController; 
	public Transform image;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Joystick Button 7"))
			{
			PauseGame ();
			}
	}
	public void PauseGame()
	{
		if(image.gameObject.activeInHierarchy == false)
		{
			image.gameObject.SetActive(true);
			Time.timeScale = 0;
		}
		else 
		{
			image.gameObject.SetActive(false);
			Time.timeScale = 1;
		}
	}
}
