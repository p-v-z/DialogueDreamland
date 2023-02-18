using UnityEditor.Animations;
using UnityEngine;

namespace DD
{
	public class Character : MonoBehaviour
	{
		// Unique identifier for this Character
		public string ID { get; private set; }
		
		private AnimatorController animatorController;
	}
}
