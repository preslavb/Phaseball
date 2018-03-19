using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class BallManager
{
	public static bool hasBallBeenTouched = false;
}

namespace UnityStandardAssets._2D
{
	public class PlatformerCharacter2D : MonoBehaviour
	{
		public int playerNumber = 1;
		int playerWithBall;

		bool jumped;

		public TimeManager timeManager;

		[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
		[SerializeField] public float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
		[SerializeField] public float m_BallForce = 10f;
		[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
		[SerializeField] private float m_JumpCooldown = 0.5f;
		[SerializeField] private float m_BallCooldown = 0.2f;
		[SerializeField] private float m_FallCooldown = 2f;
		[SerializeField] private GameObject m_BallPrefab;

		private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
		private Transform m_LeftCheck;
		private Transform m_RightCheck;
		const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool m_Grounded;            // Whether or not the player is grounded.
		private Transform m_CeilingCheck;   // A position marking where to check for ceilings
		const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
		private Animator m_Anim;            // Reference to the player's animator component.
		private AudioSource audioSource;
		private Rigidbody2D m_Rigidbody2D;
		public bool m_FacingRight = true;  // For determining which way the player is currently facing.

		public bool hasBallControl = false;
		public float defaultGravity = 10;
		public bool slowTime = false; // used to keep track if time is slow or not.

		public Team teamBelongingTo;
		private Vector2 startPosition;
		private bool bumped;

		private void Awake()
		{
			// Setting up references.
			m_GroundCheck = transform.Find("GroundCheck");
			m_LeftCheck = transform.Find("GroundCheck (1)");
			m_RightCheck = transform.Find("GroundCheck (2)");
			m_CeilingCheck = transform.Find("CeilingCheck");
			
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			audioSource = GetComponent<AudioSource>();
			startPosition = transform.position;

			timeManager = GameObject.Find("Main Camera").GetComponent<TimeManager>();
		}

		private void Update()
		{
			/*
			if(this.jumped == true)
			{
				print ("Player "  + this.playerNumber.ToString() + " is no longer on the ground");
			}
			//Test: Toggle timescale
			if(Input.GetButtonDown("Fire1"))
			{
				if (!slowTime) 
				{
					slowTime = true;
					timeManager.SlowDownTime();
				} 
				else 
				{
					slowTime = false;
					timeManager.NormalTime ();
				}
			}
			*/

			if (m_JumpCooldown > 0)
			{
				m_JumpCooldown -= Time.deltaTime;
			}

			if (m_BallCooldown > 0)
			{
				m_BallCooldown -= Time.deltaTime;
			}

			if (m_Grounded && !hasBallControl)DrawParabola((new Vector2(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber)).normalized), 10);
		
			if (hasBallControl) DrawThrowLine(new Vector2(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber)).normalized);
		}

		private void FixedUpdate()
		{
			m_Anim.SetBool("Ball", hasBallControl);
			m_Anim.SetBool("Ground", m_Grounded);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				m_Rigidbody2D.gravityScale = defaultGravity;
				m_Grounded = false;
			}
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				m_Grounded = true;
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.layer != 8)
			{
				m_Rigidbody2D.velocity = Vector2.zero;
				m_Grounded = true;
				jumped = false;
				m_Rigidbody2D.gravityScale = 0;
				m_FallCooldown = 2;

				ContactPoint2D[] contactPoints = new ContactPoint2D[1];
				collision.GetContacts(contactPoints);

				if (Mathf.Abs(contactPoints[0].normal.x) > Mathf.Abs(contactPoints[0].normal.y))
				{
					m_Anim.SetBool("Wall", true);
				}
				else if (contactPoints[0].normal.y < 0)
				{
					m_Anim.SetBool("Ceiling", true);
				}

				if (hasBallControl)
				{
					timeManager.NormalTime();
					slowTime = false;
					//print ("Normal time colision not ball, Player with ball: " + this.playerNumber.ToString());
				}
			}

			else if (collision.gameObject.name.Contains("Player") && !bumped)
			{
				m_Rigidbody2D.velocity = -m_Rigidbody2D.velocity;
				m_Rigidbody2D.gravityScale = defaultGravity;

				collision.gameObject.GetComponent<PlatformerCharacter2D>().m_Rigidbody2D.velocity = -m_Rigidbody2D.velocity;
				collision.gameObject.GetComponent<PlatformerCharacter2D>().m_Rigidbody2D.gravityScale = defaultGravity;

				collision.gameObject.GetComponent<PlatformerCharacter2D>().bumped = true;
				bumped = true;

				if (collision.gameObject.GetComponent<PlatformerCharacter2D>().hasBallControl)
				{
					this.hasBallControl = true;
					collision.gameObject.GetComponent<PlatformerCharacter2D>().hasBallControl = false;
				}

				m_Grounded = false;
				collision.gameObject.GetComponent<PlatformerCharacter2D>().m_Grounded = false;
			}
		}

		private void LateUpdate()
		{
			bumped = false;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.name == "BallPickupTrigger" && m_BallCooldown <= 0)
			{
				hasBallControl = true;

				playerWithBall = this.playerNumber;

				if (this.jumped == true)
				{
					timeManager.SlowDownTime();
					slowTime = true;
				}

				BallManager.hasBallBeenTouched = true;

				m_BallCooldown = timeManager.slowFactor/10;

				other.gameObject.transform.parent.GetComponent<BallScript>().Destroy();
			}
		}

		public void Move(Vector2 jumpDirection, bool jump)
		{

			// If the player should jump...
			if (m_Grounded && jump && m_Anim.GetBool("Ground") && m_JumpCooldown <= 0 && !hasBallControl)
			{
				// Add a vertical force to the player.
				m_Grounded = false;
				jumped = true;
				m_JumpCooldown = 0.5f;
				m_Anim.SetBool("Ground", false);
				m_Anim.SetBool("Wall", false);
				m_Anim.SetBool("Ceiling", false);
				audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Jump"));
				m_Rigidbody2D.velocity = (jumpDirection * m_JumpForce);

				if (((jumpDirection.x > 0 && !m_FacingRight) || (jumpDirection.x < 0 && m_FacingRight)) && !m_Anim.GetBool("Wall"))
				{
					Flip();
				}

				m_FallCooldown = 0;
				m_Rigidbody2D.gravityScale = defaultGravity;
			}

			else if (hasBallControl && jump && m_BallCooldown <= 0)
			{

				hasBallControl = false;

				timeManager.NormalTime();

				slowTime = false;

				audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Shoot"));

				if (((jumpDirection.x > 0 && !m_FacingRight) || (jumpDirection.x < 0 && m_FacingRight)) && !m_Anim.GetBool("Wall"))
				{
					Flip();
				}

				GameObject ballInstance = Instantiate(m_BallPrefab);

				ballInstance.transform.position = gameObject.transform.position;
				ballInstance.name = "Ball";
				ballInstance.GetComponent<Rigidbody2D>().velocity = jumpDirection * m_BallForce;

				m_BallCooldown = 0.05f;
				m_JumpCooldown = 0.5f;
			}
		}


		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;

			gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
		}

		private void DrawParabola(Vector2 jumpDirection, int complexityPerSecond, float numberOfSeconds = 3)
		{
			List<Vector2> parabolaVertices = new List<Vector2>();

			parabolaVertices.Add((Vector2)gameObject.transform.position);

			for (float i = 0; i < complexityPerSecond * numberOfSeconds; i += 0.1f)
			{

				parabolaVertices.Add((Vector2)gameObject.transform.position + FindPointOnParabola(jumpDirection * m_JumpForce, i));

			}
			//Debug.Log(jumpDirection + ", " + m_JumpForce + ", " + m_Rigidbody2D.velocity.magnitude);
			
			for (int i = 1; i < parabolaVertices.Count; i++)
			{
				Debug.DrawLine(parabolaVertices[i - 1], parabolaVertices[i]);
			}
		}

		private Vector2 FindPointOnParabola(Vector2 initialVelocity, float time, float gravityAcceleration = -9.81f)
		{
			return new Vector2(initialVelocity.x * time, (initialVelocity.y) * time + (gravityAcceleration * time * time));
		}

		private void DrawThrowLine(Vector2 jumpDirection)
		{
			Debug.DrawLine((Vector2)gameObject.transform.position, (Vector2)gameObject.transform.position + jumpDirection * 100);
		}

		public void ResetPosition()
		{
			transform.position = startPosition;
			m_Rigidbody2D.velocity = Vector2.zero;
			m_Anim.SetBool("Wall", false);
			m_Anim.SetBool("Ceiling", false);
			m_Anim.SetBool("Ground", false);
		}
	}
}
