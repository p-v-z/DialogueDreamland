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
		[Required, SerializeField] private CConfig config;
		[Required, SerializeField] private Animator animator; // Reference to animator component

		private Rigidbody rb; // Reference to player's Rigidbody component
		
		private static readonly int IsWalking = Animator.StringToHash("IsWalking");

		private void Awake()
		{
			if (rb == null)
			{
				rb = GetComponent<Rigidbody>(); // Get the Rigidbody component on start
			}
		}

		private void FixedUpdate()
		{
			animator.SetBool(IsWalking, rb.velocity.magnitude != 0);
		}

		public void HandleCharacterMovement(Vector2 movement)
		{
			// Normalize movement vector to have consistent speed in all directions
			var normalized = movement.normalized * config.moveSpeed;
			
			// Don't use velocity to move X and Z axis
			rb.MovePosition(rb.position + new Vector3(normalized.x, 0, normalized.y) * Time.fixedDeltaTime);
		}
	}
}
