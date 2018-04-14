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

	public static BoostRespawn Instance;

	// Use this for initialization
	void Start () 
	{
		Instance = this;
		spawnBoost();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (respawnTimer <= respawnTime && canRespawn)
		{
			respawnTimer += Time.deltaTime;
		}
		else if (respawnTimer >= respawnTime && canRespawn)
		{
			spawnBoost ();
		}
	}

	public void resetTimer()
	{
		respawnTimer = 0;
		canRespawn = true;
	}

	public void spawnBoost()
	{
		canRespawn = false;

		foreach (Transform spawnPoint in spawnPoints)
		{
			if (spawnPoint.childCount == 0)
				Instantiate(boostPrefab[0], spawnPoint.transform.position, Quaternion.Euler(0, 0, 0), spawnPoint);
		}
		//boostPickUp[0] = Instantiate(boostPrefab[0], spawnPoints[0].transform.position,Quaternion.Euler(0,0,0))as GameObject;
	}
}
