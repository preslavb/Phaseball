using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {
	public Platformer2DUserControl playerController; 
	public Transform pauseMenu;
	public Transform optionsMenu;

	public static int PlayerPausedIndex = 1;
	public static PauseMenu Instance;

	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	public void PauseGame(Platformer2DUserControl characterPaused)
	{
		if(pauseMenu.gameObject.activeInHierarchy == false)
		{
			pauseMenu.gameObject.SetActive(true);
			optionsMenu.gameObject.SetActive (false);
			Time.timeScale = 0;
			Platformer2DUserControl.controllable = false;
			characterPaused.thisPlayerPaused = true;
			playerController = characterPaused;
		}
		else 
		{
			pauseMenu.gameObject.SetActive(false);
			optionsMenu.gameObject.SetActive (false);
			Time.timeScale = 1;
			Platformer2DUserControl.controllable = true;
			characterPaused.thisPlayerPaused = false;
		}
	}

	public void Unpause()
	{
		pauseMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(false);
		Time.timeScale = 1;
		Platformer2DUserControl.controllable = true;
		playerController.thisPlayerPaused = false;
	}

	public void Quit()
	{
		SceneManager.LoadScene (0);
	}
}
