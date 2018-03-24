using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;

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





	public void Quit()
	{
		SceneManager.LoadScene (0);
	}
}
