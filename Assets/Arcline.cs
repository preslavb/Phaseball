﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;
using UnityStandardAssets;


public class Arcline : MonoBehaviour {

    
    public GameObject selectedObject;
   
    public bool Arc;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetAxis("Horizontal1") != 0 &&  Arc == false)
        {
            
            Arc = true;
        }
        

        if (Input.GetAxis ("Vertical1") != 0 && Arc == false)
        {

            Arc = true;
        }

    }

    private void OnDisable()
    {
        Arc = false;
    }
}
