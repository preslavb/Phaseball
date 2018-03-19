using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PauseMenu : MonoBehaviour {
	public Platformer2DUserControl playerController; 
	public Transform pauseMenu;
	public Transform optionsMenu;

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
		if(pauseMenu.gameObject.activeInHierarchy == false)
		{
			pauseMenu.gameObject.SetActive(true);
			optionsMenu.gameObject.SetActive (false);
			Time.timeScale = 0;
		}
		else 
		{
			pauseMenu.gameObject.SetActive(false);
			optionsMenu.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}

	public void Options(bool Selected)
	{
		if (Selected) 
		{
			optionsMenu.gameObject.SetActive (true);
			pauseMenu.gameObject.SetActive(false);

		}
		if (!Selected) 
		{
			optionsMenu.gameObject.SetActive (false);
			pauseMenu.gameObject.SetActive (true);

		}
	}

	public void Back(bool Clicked)
	{
		if (Clicked)
		{
			optionsMenu.gameObject.SetActive (false);
			pauseMenu.gameObject.SetActive(true);

		}

		if (!Clicked) 
		{
			optionsMenu.gameObject.SetActive (true);
			pauseMenu.gameObject.SetActive(false);

		}
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
