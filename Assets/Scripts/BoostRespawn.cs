using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to a boost spawned game object, handles the spawning of boost pickups
public class BoostRespawn : MonoBehaviour 
{

	// The current time passed after the last respawn
	public float respawnTimer = 0.0f;

	// The time at which the boosts should respawn
	public float respawnTime = 15;

	// Whether respawning is enabled for boosts
	bool canRespawn = false;

	// Array of spawn points where the boosts should be spawned at
	public Transform[]  spawnPoints;

	// The prefabs used for spawning boosts
	public GameObject[] boostPrefab;

	// The singleton instance of the boost spawner
	public static BoostRespawn Instance;

	// Upon initialize of the level, load the instance and spawn the first set of boosts
	void Start () 
	{
		Instance = this;
		SpawnBoost();
	}
	
	// Handle the timer for respawning
	void Update () 
	{

		// If respawning is enabled, but it is still not time to spawn the boosts, tick up the timer by deltaTime
		if (respawnTimer <= respawnTime && canRespawn)
		{
			respawnTimer += Time.deltaTime;
		}

		// If respawning is enabled and the timer has reached or passed the threshold for spawning, spawn the boosts
		else if (respawnTimer >= respawnTime && canRespawn)
		{
			SpawnBoost ();
		}
	}

	// Reset the timer and allow for boosts to spawn once more
	public void ResetTimer()
	{
		if (!canRespawn)
		{
			respawnTimer = 0;
			canRespawn = true;
		}
	}

	// Spawn the boosts and stop the timer from ticking (will be restarted when one of the boosts has been picked up)
	public void SpawnBoost()
	{
		canRespawn = false;

		foreach (Transform spawnPoint in spawnPoints)
		{
			if (spawnPoint.childCount == 0)
				Instantiate(boostPrefab[0], spawnPoint.transform.position, Quaternion.Euler(0, 0, 0), spawnPoint);
		}
	}
}
