using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Remove from namespace
namespace UnityStandardAssets._2D
{

	// TODO: Rename to more sensible name
	public class PlatformerCharacter2D : MonoBehaviour
	{

		// The index of the player
		public int playerNumber = 1;

		// How many boosts they can carry
		public int maxBoost = 1;

		// The amount of boost they have
		public int currentBoost = 0;

		// Whether the player has reached their maximum boost capacity
		bool canPickUp = true;
		
		// Whether the player has enough boost to boost
		bool canBoost = false;

		// Flag for jump TODO: use the Control jump flag instead of this one
		bool jumped;

		// Reference to the time manager (for slowing down time when the player picks up a ball
		public TimeManager timeManager;

		// Reference to the boost spawner (for respawning all boosts when a goal is scored
		public BoostRespawn boostRespawn;

		// The sprite responsible for displaying this player's boost
		public Image boostSprite;

		// The boost states sprites for the UI
		public Sprite boostAvailable;
		public Sprite boostEmpty;

		public float jumpForce = 400f;              // Amount of force added when the player jumps
		public float ballForce = 10f;				// Force with which to shoot the ball
		private float jumpCooldown = 0.5f;			// Amount of time to wait after allowing another jump TODO: Should tweak
		private float ballCooldown = 0.2f;			// Amount of time to wait after allowing the player to throw the ball after catching it

		// References for the ball
		public GameObject blueTeamBall;
		public GameObject redTeamBall;

		private bool isGrounded;					// Whether or not the player is grounded.
		private Animator characterAnimator;         // Reference to the player's animator component.
		private AudioSource audioSource;			// Reference to the audio source
		private Rigidbody2D rigidBody2D;			// Reference to the rigid body component

		public bool isFacingRight = true;			// For determining which way the player is currently facing.

		// Whether this player is currently in control of the ball
		public bool hasBallControl = false;

		// The default gravity scale for a character
		public float defaultGravity = 10;

		// Whether time should slow or not TODO:(should be moved out of this class)
		public bool slowTime = false;

		// The team this character belongs to
		public Team teamBelongingTo;

		// The position the character should respawn at when a goal is scored
		private Vector2 startPosition;

		// If the character was bumped by another character and has not landed on a floor or wall
		private bool bumped;

		private void Awake()
		{

			// Setting up references.			
			characterAnimator = GetComponent<Animator>();
			rigidBody2D = GetComponent<Rigidbody2D>();
			audioSource = GetComponent<AudioSource>();
			startPosition = transform.position;

			timeManager = GameObject.Find("Main Camera").GetComponent<TimeManager>();
			boostRespawn = GameObject.Find("BoostSpawner").GetComponent<BoostRespawn>();
		}

		private void Update()
		{
			canPickUp = true;

			// Disallow boosting if the player isn't already in the air
			if (jumped) 
			{
				canBoost = true;
			}

			// Tick down the jump cooldown
			if (jumpCooldown > 0)
			{
				jumpCooldown -= Time.unscaledDeltaTime;
			}

			// Tick down the ball cooldown
			if (ballCooldown > 0)
			{
				ballCooldown -= Time.unscaledDeltaTime;
			}

			// Set the appropriate animator states based on the player state TODO: Should be moved to a function
			characterAnimator.SetBool("Ball", hasBallControl);
			characterAnimator.SetBool("Ground", isGrounded);

			// DEBUG
			//if (isGrounded && !hasBallControl) DrawDebugParabula((new Vector2(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber)).normalized), 10);

			//if (hasBallControl) DrawDebugThrowLine(new Vector2(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber)).normalized);
		}

		// When exiting out of a collision with a level object, reenable gravity and remove the grounded attribute
		private void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				rigidBody2D.gravityScale = defaultGravity;
				isGrounded = false;
			}
		}

		// While the player is in contact with a level object, they should stay grounded
		private void OnCollisionStay2D(Collision2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				isGrounded = true;
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			// If the player collides with an object that is not another player, they should stick to that surface
			if (collision.gameObject.layer != 8)
			{

				rigidBody2D.velocity = Vector2.zero;
				isGrounded = true;
				jumped = false;

				// Disable gravity to force player to stick to walls
				rigidBody2D.gravityScale = 0;

				// Boosting should be disallowed when the player is grounded
				canBoost = false;

				// Load a contact point for the collision
				ContactPoint2D[] contactPoints = new ContactPoint2D[1];
				collision.GetContacts(contactPoints);

				// If the angle of the contact normal is horizontal (within 45 degrees of 0), then the player hit a wall, so set the appropriate animator state
				if (Mathf.Abs(contactPoints[0].normal.x) > Mathf.Abs(contactPoints[0].normal.y))
				{
					characterAnimator.SetBool("Wall", true);
				}

				// If the angle of the contact normal is pointing downwards and is not within 45 degrees, then the player hit a ceiling
				else if (contactPoints[0].normal.y < 0)
				{
					characterAnimator.SetBool("Ceiling", true);
				}

				// If the player has ball control when landing, reset timescale to normal time
				if (hasBallControl)
				{
					timeManager.NormalTime();
				}
			}

			// If the player bumped into another player, and they have not already had the bump logic fire
			else if (collision.gameObject.name.Contains("Player") && !bumped && !collision.gameObject.GetComponent<PlatformerCharacter2D>().bumped)
			{

				// Impart the velocity of this player onto the other, and vice versa, also knocking them off of walls in the process
				collision.gameObject.GetComponent<Rigidbody2D>().velocity = rigidBody2D.velocity;
				rigidBody2D.velocity = - rigidBody2D.velocity;
				rigidBody2D.gravityScale = defaultGravity;

				// If one of the players holds ball control, and have not had the bumped logic run, transfer ball control
				if (collision.gameObject.GetComponent<PlatformerCharacter2D>().hasBallControl && !collision.gameObject.GetComponent<PlatformerCharacter2D>().bumped)
				{
					this.hasBallControl = true;
					collision.gameObject.GetComponent<PlatformerCharacter2D>().hasBallControl = false;
				}

				else if (hasBallControl && !bumped)
				{
					this.hasBallControl = false;
					collision.gameObject.GetComponent<PlatformerCharacter2D>().hasBallControl = true;
				}

				// Register the characters have already run the bumped logic, and should not run it again until they hit the ground
				bumped = true;
				collision.gameObject.GetComponent<PlatformerCharacter2D>().bumped = true;

				// isGrounded = false;

				// Check if the player has landed after the bump and reset it
				StartCoroutine(ResetBumped(collision.gameObject.GetComponent<PlatformerCharacter2D>()));
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			
			// If the player enters the ball pickup trigger (which is attached to the ball in the form of a trigger as to not interfere with player physics), pick up the ball
			if (other.gameObject.name == "BallPickupTrigger" && ballCooldown <= 0)
			{

				hasBallControl = true;
				
				// If the player is mid jump, then time should be slowed down
				if (this.jumped == true)
				{
					timeManager.SlowDownTime();
				}

				// Set a cooldown for throwing the ball TODO: Should be revisited to see if this is necessary and why
				ballCooldown = timeManager.slowFactor/10;

				// Destroy the scene ball object
				other.gameObject.transform.parent.GetComponent<BallScript>().Destroy();
			}
			
			// If the player has hit a boost pick up, then gain a boost
			if(other.gameObject.tag == "PickUp" && currentBoost < maxBoost && canPickUp)
			{
				canPickUp = false;
				GetBoost();
				Destroy(other.gameObject);
			}
		}

		// Increment boost and make sure the boost spawner resets its timer (if not currently ticking)
		void GetBoost()
		{
			currentBoost++;
			boostSprite.sprite = boostAvailable;
			boostRespawn.ResetTimer();
		}

		// Function to handle moving
		public void Move(Vector2 jumpDirection, bool jump)
		{

			// If the player should jump...
			if (isGrounded && jump && characterAnimator.GetBool("Ground") && jumpCooldown <= 0 && !hasBallControl)
			{
				// Set the appropriate flags
				isGrounded = false;
				jumped = true;

				// Set the jump cooldown
				jumpCooldown = 0.5f;

				// Reset the animator state
				characterAnimator.SetBool("Ground", false);
				characterAnimator.SetBool("Wall", false);
				characterAnimator.SetBool("Ceiling", false);

				// Play the appropriate jump SFX
				audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Jump"));

				// Add the vertical velocity of the jump
				rigidBody2D.velocity = (jumpDirection * jumpForce);

				// If the player is jumping in the opposite direction of their sprite's facing, flip the sprite
				if (((jumpDirection.x > 0 && !isFacingRight) || (jumpDirection.x < 0 && isFacingRight)) && !characterAnimator.GetBool("Wall"))
				{
					Flip();
				}

				// Enable gravity while the player is in the air
				rigidBody2D.gravityScale = defaultGravity;

			}

			// The player has the ball, so run the throw logic instead
			else if (hasBallControl && jump && ballCooldown <= 0)
			{
				
				// Unset ball control
				hasBallControl = false;

				// Set time to normal if it hasn't already been
				timeManager.NormalTime();

				// Play the SFX
				audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Shoot"));

				// Flip the sprite if shooting in the opposite direction (the wall animation currently finishes with an inverted frame, so don't rotate in that instance)
				if (((jumpDirection.x > 0 && !isFacingRight) || (jumpDirection.x < 0 && isFacingRight)) && !characterAnimator.GetBool("Wall"))
				{
					Flip();
				}

				// Initialize the new ball game object
				GameObject ballInstance;

				// Determine which color ball should be instantiated
				if (teamBelongingTo == Team.Blue)
				{
					ballInstance = Instantiate(blueTeamBall);
				}

				else
				{
					ballInstance = Instantiate(redTeamBall);
				}

				// Set the ball position to the current player
				ballInstance.transform.position = gameObject.transform.position;

				// Rename the ball for certain collision checks TODO:(should be revisited and made to be unnecessary)
				ballInstance.name = "Ball";

				// Set the ball velocity
				ballInstance.GetComponent<Rigidbody2D>().velocity = jumpDirection * ballForce;

				// Store this character as the last character to have handled the ball
				CameraController.lastHandledBy = this.gameObject;

				// Set the cooldowns
				ballCooldown = 0.05f;
				jumpCooldown = 0.5f;
			}

			// If the character has a boost and is not on the ground, then use the boost
			else if (!isGrounded && jump && !characterAnimator.GetBool("Ground") && currentBoost > 0 && currentBoost <= maxBoost && !hasBallControl && canBoost)
			{ 
				
				// Decrement the current boost count of the palyer
				currentBoost--;

				// Set the UI element to the empty variant
				boostSprite.sprite = boostEmpty;

				// Not sure if this is necessary
				isGrounded = false;
				jumpCooldown = 0.5f;
				characterAnimator.SetBool("Ground", false);
				characterAnimator.SetBool("Wall", false);
				characterAnimator.SetBool("Ceiling", false);

				// Play the jump sound effect
				audioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Jump"));

				// Add the vertical velocity
				rigidBody2D.velocity = (jumpDirection * jumpForce);

				// If jumping in the opposite direction from the character's facing, flip the sprite
				if (((jumpDirection.x > 0 && !isFacingRight) || (jumpDirection.x < 0 && isFacingRight)) && !characterAnimator.GetBool("Wall"))
				{
					Flip();
				}

				// Not sure this is relevant
				rigidBody2D.gravityScale = defaultGravity;
			}

		}


		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			isFacingRight = !isFacingRight;

			// Flip the sprite
			gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
		}

		private void DrawDebugParabula(Vector2 jumpDirection, int complexityPerSecond, float numberOfSeconds = 3)
		{
			List<Vector2> parabolaVertices = new List<Vector2>();

			parabolaVertices.Add((Vector2)gameObject.transform.position);

			for (float i = 0; i < complexityPerSecond * numberOfSeconds; i += 0.1f)
			{

				parabolaVertices.Add((Vector2)gameObject.transform.position + FindPointOnParabola(jumpDirection * jumpForce, i));

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

		private void DrawDebugThrowLine(Vector2 jumpDirection)
		{
			Debug.DrawLine((Vector2)gameObject.transform.position, (Vector2)gameObject.transform.position + jumpDirection * 100);
		}

		// Reset the position of the character to their initial position
		public void ResetPosition()
		{
			transform.position = startPosition;
			rigidBody2D.velocity = Vector2.zero;
			characterAnimator.SetBool("Wall", false);
			characterAnimator.SetBool("Ceiling", false);
			characterAnimator.SetBool("Ground", false);
		}

		// 1 second after getting bumped, allow bumping to occur again (not sure if necessary)
		System.Collections.IEnumerator ResetBumped(PlatformerCharacter2D character)
		{
			yield return new WaitForSecondsRealtime(1);
			character.bumped = false;
			bumped = false;
		}
	}
}
