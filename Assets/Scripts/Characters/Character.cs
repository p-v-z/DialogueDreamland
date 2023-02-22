using UnityEngine;

namespace DD
{
	/// <summary>
	/// The Character class is the base class for all characters in the game.
	/// </summary>
	public class Character : MonoBehaviour
	{
		// Unique identifier for this Character
		public string ID { get; private set; }
		
		[SerializeField] private CharacterController controller;
	}
}
