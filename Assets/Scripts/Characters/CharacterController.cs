using Lightbug.CharacterControllerPro.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DD
{
	/// <summary>
	/// A CharacterController is a component that handles the movement of a Character.
	/// This includes the movement of the Character's Rigidbody and the animation of the Character.
	/// </summary>
	public class CharacterController : MonoBehaviour
	{
		[Required, SerializeField] private Animator animator; // Reference to animator component

		private Rigidbody rb; // Reference to player's Rigidbody component
		
		private CharacterActor actor;
		private static readonly int IsWalking = Animator.StringToHash("IsWalking");

		private void Awake() 
		{
			if (rb == null)
			{
				rb = GetComponent<Rigidbody>(); // Get the Rigidbody component on start
			}
			actor = GetComponent<CharacterActor>();
		}
	}
}
