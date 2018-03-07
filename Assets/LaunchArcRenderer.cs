using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;
using UnityEngine.EventSystems;
using UnityStandardAssets;
using UnityStandardAssets._2D;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour
{
	//
	LineRenderer lr;

	public Platformer2DUserControl playerController;
	public PlatformerCharacter2D playerCharacterScript;
	public LayerMask layerMask;
 
	public float velocity;
	public float angle;
	public int resolution = 20;


	float g; // force of gravity of y axis
	float radianAngle;
	void Awake()
	{
		playerController = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
		playerCharacterScript = gameObject.transform.parent.GetComponent<PlatformerCharacter2D>();
		lr = GetComponent<LineRenderer>();
		g = Mathf.Abs(Physics2D.gravity.y);
	}


  


	// Use this for initialization
	void Start()
	{
		RenderArc();
	}

	void RenderArc()
	{
		lr.SetVertexCount(playerCharacterScript.hasBallControl ?  3 : resolution + 1);
		lr.SetPositions(playerCharacterScript.hasBallControl ? CalculateReflectArray() : CalculateArcArray());
	}

	private Vector2 FindPointOnParabola(Vector2 initialVelocity, float time, float gravityAcceleration = -9.81f)
	{
		return new Vector2(initialVelocity.x * time, (initialVelocity.y + ((gravityAcceleration * 10)/2) * time) * time);
	}

	Vector3[] CalculateArcArray()
	{
		List<Vector3> arcArray = new List<Vector3>(resolution * 4 + 1);

		for (int i = 0; i <= resolution * 4; i++)
		{
			float t = (float)i / (float)resolution;
			arcArray.Add(FindPointOnParabola(new Vector2(playerController.h, playerController.v) * playerCharacterScript.m_JumpForce, t));
		}

		return arcArray.ToArray();
	}

	Vector3[] CalculateReflectArray()
	{
		Vector3[] pointsArray = new Vector3[3];

		Vector2 direction = new Vector2(playerController.h, playerController.v);

		pointsArray[0] = Vector3.zero;
		//pointsArray[1] = new Vector3(20, 0, 0);
		//pointsArray[2] = new Vector3(20, 20, 0);

		RaycastHit2D raycast1Hit = Physics2D.Raycast(playerCharacterScript.gameObject.transform.position, direction, Mathf.Infinity, layerMask);

		if (raycast1Hit)
		{
			Vector2 raycast1Point = Vector2.Lerp((Vector2)playerCharacterScript.gameObject.transform.position, raycast1Hit.point, .98f);

			pointsArray[1] = raycast1Point - (Vector2)playerCharacterScript.gameObject.transform.position;

			RaycastHit2D raycast2Hit = Physics2D.Raycast(raycast1Point, Vector2.Reflect(direction, raycast1Hit.normal), Mathf.Infinity, layerMask);
			
			if (raycast2Hit)
			{
				pointsArray[2] = raycast2Hit.point - (Vector2)playerCharacterScript.gameObject.transform.position;
			}
		}

		return pointsArray;
	}

	void Update()
	{
	   
		//angle = Vector2.SignedAngle(Vector2.right, new Vector2(playerCharacterScript.h, playerCharacterScript.v));
		if (lr != null && Application.isPlaying)
		{
			RenderArc();
		}

	}






}



