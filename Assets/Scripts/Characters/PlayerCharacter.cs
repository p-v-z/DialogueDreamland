using UnityEngine;

namespace DD
{
	/// <summary>
	/// A Character that is controlled by the player.
	/// </summary>
	public class PlayerCharacter : Character
	{
		[SerializeField] private PlayerController playerController;

		
		public void OnEnable()
		{
			Debug.Log("Player character enabled");
			// playerController.OnMove += playerController.HandleCharacterMovement;
			// actor.Move();
		}

		// TODO: Add way to interact with NPC
	}
}