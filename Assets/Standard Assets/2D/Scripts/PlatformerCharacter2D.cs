using System;
using System.Collections.Generic;
using UnityEngine;

public static class BallManager
{
    public static bool hasBallBeenTouched = false;
}

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        public int playerNumber = 1;
		public TimeManager timeManager;

        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
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
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private bool hasBallControl = false;
		public bool slowTime = false; // used to keep track if time is slow or not.
        
		private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_LeftCheck = transform.Find("GroundCheck (1)");
            m_RightCheck = transform.Find("GroundCheck (2)");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
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

            if (m_JumpCooldown > 0)
            {
                m_JumpCooldown -= Time.deltaTime;
            }

            if (m_BallCooldown > 0)
            {
                m_BallCooldown -= Time.deltaTime;
            }

            if (m_FallCooldown > 0)
            {
                m_FallCooldown -= Time.deltaTime;
            }
            else
            {
                m_Rigidbody2D.gravityScale = 10;
            }

			if (m_Grounded && !hasBallControl)DrawParabola((new Vector2(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber))), 10);
		
			if (hasBallControl) DrawThrowLine(new Vector2(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber)));
        }

        private void FixedUpdate()
        { 

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }

            //colliders = Physics2D.OverlapCircleAll(m_LeftCheck.position, k_GroundedRadius, m_WhatIsGround);
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    if (colliders[i].gameObject != gameObject)
            //    {
            //        m_Grounded = true;
            //        m_Rigidbody2D.gravityScale = 0;
            //        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.);
            //    } 
            //}

            //colliders = Physics2D.OverlapCircleAll(m_RightCheck.position, k_GroundedRadius, m_WhatIsGround);
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    if (colliders[i].gameObject != gameObject)
            //    {
            //        m_Grounded = true;
            //        m_Rigidbody2D.gravityScale = 0;
            //        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            //    }
            //}

            //colliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, k_GroundedRadius, m_WhatIsGround);
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    if (colliders[i].gameObject != gameObject)
            //    {
            //        m_Grounded = true;
            //        m_Rigidbody2D.gravityScale = 0;
            //        m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
            //    }
            //}

            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            
            //Debug.DrawLine(gameObject.transform.position, (Vector2)gameObject.transform.position + (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) * m_JumpForce);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Ball" && m_BallCooldown <= 0)
            {
                hasBallControl = true;

				timeManager.SlowDownTime ();

				slowTime = true;


				//Time.timeScale = 0.1f;

                BallManager.hasBallBeenTouched = true;

                m_BallCooldown = 0.05f;

                Destroy(collision.gameObject);
            }

            if (collision.gameObject.name != "Ball" && collision.gameObject.layer != 8)
            {
                m_Rigidbody2D.velocity = Vector2.zero;
                m_Grounded = true;
                m_Rigidbody2D.gravityScale = 0;
                m_FallCooldown = 2;
            }
        }


        public void Move(Vector2 jumpDirection, bool jump)
        {
			if (m_Grounded) // needs to activate for each player
			{
				//slowTime = false;
				//timeManager.NormalTime ();
			} 
				
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground") && m_JumpCooldown <= 0 && !hasBallControl)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_JumpCooldown = 0.5f;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.velocity = (jumpDirection * m_JumpForce);

                m_FallCooldown = 0;
            }

            else if (hasBallControl && jump && m_BallCooldown <= 0)
            {
                //TODO: Implement throwing
                hasBallControl = false;

				timeManager.NormalTime();

				slowTime = false;


                GameObject ballInstance = Instantiate(m_BallPrefab);

                ballInstance.transform.position = gameObject.transform.position;
                ballInstance.name = "Ball";
                ballInstance.GetComponent<Rigidbody2D>().velocity = jumpDirection * m_JumpForce;

                m_BallCooldown = 0.2f;
                m_JumpCooldown = 0.5f;

                Time.timeScale = 1;
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private void DrawParabola(Vector2 jumpDirection, int complexityPerSecond, float numberOfSeconds = 3)
        {
            List<Vector2> parabolaVertices = new List<Vector2>();

            parabolaVertices.Add((Vector2)gameObject.transform.position);

            for (float i = 0; i < complexityPerSecond * numberOfSeconds; i += 0.1f)
            {

                parabolaVertices.Add((Vector2)gameObject.transform.position + FindPointOnParabola(jumpDirection * m_JumpForce / 2, i));

            }
			/////debug log 
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
    }
}
