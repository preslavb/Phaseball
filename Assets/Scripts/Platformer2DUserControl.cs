using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

// TODO: Should be removed from standard assets namespace and remove all of the needless "using"s
namespace UnityStandardAssets._2D
{

	// Require the object to have a character component in order to be controlled
	[RequireComponent(typeof (PlatformerCharacter2D))]
	
	// Handles all INPUT (no actual logic should be included) TODO: Rename to a more sensible name
	public class Platformer2DUserControl : MonoBehaviour
	{

		// A reference to the character script attached to this game object
		private PlatformerCharacter2D characterReference;

		// Whether the jump button has been pressed this frame
		private bool jump;

		// Horizontal and vertical axis values
		public float h;
		public float v;

		// Whether this player has paused the game
		public bool thisPlayerPaused = false;

		// A vector for jump direction based on the input axis of the controller
		private Vector2 jumpVector = new Vector2(0, 0);

		// Whether the players should respond to input
		public static bool controllable = true;

		// Load the references
		private void Awake()
		{
			characterReference = GetComponent<PlatformerCharacter2D>();
		}

		private void Update()
		{

			// If the pause button of this player was pressed, then tell the UI Event system which inputs it should listen to for navigating the menu
			if (CrossPlatformInputManager.GetButtonDown("PauseButton" + characterReference.playerNumber))
			{

				// Store the controller index of the player who paused
				PauseMenu.PlayerPausedIndex = characterReference.playerNumber;

				// Reset the UI Event System inputs to this player's inputs
				GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal" + characterReference.playerNumber.ToString();
				GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().verticalAxis = "Vertical" + characterReference.playerNumber.ToString();
				GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().submitButton = "Jump" + characterReference.playerNumber.ToString();

				// Run the pause menu
				PauseMenu.Instance.PauseGame(this);
			}
			
			// Make sure that if this player hasn't paused, they can still move their vector (even if another player has paused)
			if (!thisPlayerPaused)
			{
				h = CrossPlatformInputManager.GetAxis("Horizontal" + characterReference.playerNumber);
				v = CrossPlatformInputManager.GetAxis("Vertical" + characterReference.playerNumber);

				jumpVector = new Vector2(h, v);
			}

			// Only allow players to jump if they are permitted input
			if (controllable)
			{
				jump = CrossPlatformInputManager.GetButtonDown("Jump" + characterReference.playerNumber);
			}

			else
			{
				jump = false;
			}

			// Pass all parameters to the character control script.
			characterReference.Move(jumpVector, jump);

			// Reset the jump flag
			jump = false;
		}
	}
}
