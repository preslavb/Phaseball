using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        public bool m_Jump;
        public float h;
        public float v;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {

            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump" + m_Character.playerNumber);
			if (m_Jump)
			{
				Debug.Log(m_Character.playerNumber);
			}

            h = CrossPlatformInputManager.GetAxis("Horizontal" + m_Character.playerNumber);
            v = CrossPlatformInputManager.GetAxis("Vertical" + m_Character.playerNumber);

            Vector2 jumpVector = new Vector2(h, v);

            // Pass all parameters to the character control script.
            m_Character.Move(jumpVector, m_Jump);
            m_Jump = false;
        }
    }
}
