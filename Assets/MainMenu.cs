using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;


public class MainMenu : MonoBehaviour {
	public Platformer2DUserControl playerController;
	// Use this for initialization


	public void PlayLevel1()
	{
		SceneManager.LoadScene (1);
	}

	public void PlayLevel2()
	{
		SceneManager.LoadScene (2);
	}


	public void ExitGame()
	{
		Debug.Log ("Exit");
		Application.Quit ();
	}
}
