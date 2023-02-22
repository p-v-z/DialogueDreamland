using System;
using Lightbug.CharacterControllerPro.Core;
using UnityEngine;
namespace DD
{
	/// <summary>
	/// The PlayerController is a CharacterController that handles input from the player.
	/// </summary>
	[RequireComponent(typeof(CharacterActor))]
	public class PlayerController : CharacterController
	{
		

		public Action<Vector2> OnMove = delegate { }; 
		
		private void FixedUpdate()
		{
			// Get horizontal and vertical input from the player
			var moveHorizontal = Input.GetAxis("Horizontal");
			var moveVertical = Input.GetAxis("Vertical");

			// Move the player in the direction of the input
			var movement = new Vector2(moveHorizontal, moveVertical);
			// HandleCharacterMovement(movement);
			// OnMove.Invoke(movement);
		}
	}
}
