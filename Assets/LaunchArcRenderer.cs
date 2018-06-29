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
	LineRenderer lr;

	public Platformer2DUserControl playerController;
	public PlatformerCharacter2D playerCharacterScript;
	public LayerMask layerMask;
 
	public float velocity;
	public float angle;
	public int resolution = 20;

	float g;
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
		if (playerCharacterScript.hasBallControl)
		{
			lr.positionCount = 3;
			lr.SetPositions(CalculateReflectArray());
		}

		else
		{
			Vector3[] arcArray = CalculateArcArray();
			lr.positionCount = arcArray.Length;
			lr.SetPositions(arcArray);
		}
	}

	// Find the point at which the player would be if they jumped after a specified time
	private Vector2 FindPointOnParabola(Vector2 initialVelocity, float time, float gravityAcceleration = -9.81f)
	{
		return new Vector2(initialVelocity.x * time, (initialVelocity.y + ((gravityAcceleration * playerCharacterScript.defaultGravity)/2) * time) * time);
	}

	Vector3[] CalculateArcArray()
	{
		List<Vector3> arcArray = new List<Vector3>(resolution + 1);

		// The corners to also check for collisions
		Vector2 offset1 = new Vector2();
		Vector2 offset2 = new Vector2();

		Vector2 direction = new Vector2(playerController.h, playerController.v);
		Vector2 dimensions = playerCharacterScript.gameObject.GetComponent<BoxCollider2D>().size;

		// Determine which corners of the character are most likely to hit something on their way when they jump
		if (direction.x == 0)
		{
			offset1 = new Vector2(-dimensions.x / 2, direction.y > 0 ? dimensions.y / 2 : -dimensions.y / 2);
			offset2 = new Vector2(dimensions.x / 2, direction.y > 0 ? dimensions.y / 2 : -dimensions.y / 2);
		}
		else if (direction.y == 0)
		{
			offset1 = new Vector2(direction.x > 0 ? dimensions.x / 2 : -dimensions.x / 2, dimensions.y / 2);
			offset2 = new Vector2(direction.x > 0 ? dimensions.x / 2 : -dimensions.x / 2, -dimensions.y / 2);
		}
		else if (direction.y > 0)
		{
			offset1 = new Vector2(direction.x > 0 ? -dimensions.x / 2 : dimensions.x / 2, dimensions.y / 2);
			offset2 = new Vector2(direction.x > 0 ? dimensions.x / 2 : -dimensions.x / 2, -dimensions.y / 2);
		}
		else if (direction.y <= 0)
		{
			offset1 = new Vector2(direction.x > 0 ? dimensions.x / 2 : -dimensions.x / 2, dimensions.y / 2);
			offset2 = new Vector2(direction.x > 0 ? -dimensions.x / 2 : dimensions.x / 2, -dimensions.y / 2);
		}

		// Calculate the point at which the corners would hit something
		Arc end1 = CalculateArcHits(offset1);
		Arc end2 = CalculateArcHits(offset2);

		// Calculate which corner was hit first (thus making it the only one to be hit, as the character will not move further)
		Arc arcEndpoint = new Arc();
		Arc otherEndpoint = new Arc();

		if (end1.arcVertices < end2.arcVertices)
		{
			arcEndpoint = end1;
			otherEndpoint = end2;
		} else
		{
			arcEndpoint = end2;
			otherEndpoint = end1;
		}

		// The previous point checked for collisions
		Vector2 oldVector = FindPointOnParabola(direction * playerCharacterScript.jumpForce, 0);

		// Populate the arc vertices
		for (int i = 0; i <= arcEndpoint.arcVertices; i++)
		{
			float t = (float)i / (float)resolution;
			Vector2 currentVector = FindPointOnParabola(direction * playerCharacterScript.jumpForce, t);

			// Raycast between the previous vertex and the current one to see if a collision would occur
			RaycastHit2D raycastHit = Physics2D.Raycast((Vector2)playerCharacterScript.gameObject.transform.position + oldVector, currentVector - oldVector, Vector2.Distance(oldVector, currentVector));

			// There was a registered hit with a level object, so the arc is complete
			if (raycastHit && raycastHit.collider.gameObject.layer == 9)
			{
				arcArray.Add(raycastHit.point - (Vector2)playerCharacterScript.transform.position);
				break;
			}

			// We've reached the point where the other arc collided with the level, so add a vertex that is in between this one and the other arc one
			if (i == arcEndpoint.arcVertices)
			{
				arcArray.Add((Vector2.Lerp(arcEndpoint.raycastHit.point, playerCharacterScript.transform.position + otherEndpoint.vertices[i], 0.5f)) - (Vector2)playerCharacterScript.transform.position);
			}
			else
			{
				arcArray.Add(currentVector);
			}

			oldVector = currentVector;
		}

		return arcArray.ToArray();
	}

	Arc CalculateArcHits(Vector2 offset)
	{
		Arc arcEndpoint = new Arc();
		arcEndpoint.vertices = new List<Vector3>();

		Vector2 oldVector = FindPointOnParabola(new Vector2(playerController.h, playerController.v) * playerCharacterScript.jumpForce, 0);

		for (int i = 0; i <= resolution * 4; i++)
		{
			float t = (float)i / (float)resolution;
			Vector2 currentVector = FindPointOnParabola(new Vector2(playerController.h, playerController.v) * playerCharacterScript.jumpForce, t);

			RaycastHit2D raycastHit = Physics2D.Raycast((Vector2)playerCharacterScript.gameObject.transform.position + offset + oldVector, currentVector - oldVector, Vector2.Distance(oldVector, currentVector));

			if (raycastHit && raycastHit.collider.gameObject.layer == 9)
			{
				arcEndpoint.arcVertices = i;
				arcEndpoint.raycastHit = raycastHit;
				arcEndpoint.vertices.Add(raycastHit.point - (Vector2)playerCharacterScript.gameObject.transform.position);

				return arcEndpoint;
			}

			arcEndpoint.vertices.Add(currentVector);
			oldVector = currentVector;
		}

		return new Arc();
	}

	Vector3[] CalculateReflectArray()
	{
		Vector3[] pointsArray = new Vector3[3];

		Vector2 direction = new Vector2(playerController.h, playerController.v);

		pointsArray[0] = Vector3.zero;

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

public struct Arc
{
	public List<Vector3> vertices;
	public RaycastHit2D raycastHit;
	public int arcVertices;
}