using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;
using UnityStandardAssets;


public class Arcline : MonoBehaviour {

	
	public GameObject selectedObject;
   
	public bool Arc;

	// Only draw arcs if theres input
	void Update() {
		if (Input.GetAxis("Horizontal1") != 0 &&  Arc == false)
		{
			
			Arc = true;
		}
		

		else if (Input.GetAxis ("Vertical1") != 0 && Arc == false)
		{

			Arc = true;
		}

		else
		{
			Arc = false;
		}

	}

	private void OnDisable()
	{
		Arc = false;
	}
}
