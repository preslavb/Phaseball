using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof (PlatformerCharacter2D))]
	public class Platformer2DUserControl : MonoBehaviour
	{
		private PlatformerCharacter2D m_Character;
		private bool m_Jump;
		public float h;
		public float v;

		private Vector2 jumpVector = new Vector2(0, 0);

		public static bool controllable = true;


		private void Awake()
		{
			m_Character = GetComponent<PlatformerCharacter2D>();
		}


		private void Update()
		{
			if (CrossPlatformInputManager.GetButtonDown("PauseButton" + m_Character.playerNumber))
			{
				PauseMenu.PlayerPausedIndex = m_Character.playerNumber;

				GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal" + m_Character.playerNumber.ToString();
				GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>().verticalAxis = "Vertical" + m_Character.playerNumber.ToString();

				PauseMenu.Instance.PauseGame();
			}

			h = CrossPlatformInputManager.GetAxis("Horizontal" + m_Character.playerNumber);
			v = CrossPlatformInputManager.GetAxis("Vertical" + m_Character.playerNumber);

			jumpVector = new Vector2(h, v);

			if (controllable)
			{
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown("Jump" + m_Character.playerNumber);
			}

			else
			{
				m_Jump = false;
			}

			// Pass all parameters to the character control script.
			m_Character.Move(jumpVector, m_Jump);
			m_Jump = false;
		}
	}
}
