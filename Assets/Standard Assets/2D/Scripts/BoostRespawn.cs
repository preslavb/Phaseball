using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRespawn : MonoBehaviour 
{
	public float respawnTimer = 0.0f;
	public float respawnTime = 15;

	bool canRespawn = false;

	public Transform[]  spawnPoints;
	public GameObject[] boostPrefab;
	public GameObject[] boostPickUp;


	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(respawnTimer <= respawnTime&& canRespawn)
		{
			respawnTimer += Time.deltaTime;
		}
		else if (respawnTimer >= respawnTime&& canRespawn)
		{
			spawnBoost ();
		}
	}

	public void resetTimer()
	{
		respawnTimer = 0;
		canRespawn = true;
	}

	void spawnBoost()
	{
		canRespawn = false;
		boostPickUp[0] = Instantiate(boostPrefab[0], spawnPoints[0].transform.position,Quaternion.Euler(0,0,0))as GameObject;
	}
}
