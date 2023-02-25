using UnityEditor.Animations;
using UnityEngine;

namespace DD
{
	[CreateAssetMenu(fileName = "New Character Config", menuName = "CConfig")]
	public class CConfig : ScriptableObject
	{
		public AnimatorController animatorController;
		public float moveSpeed = 5f;
		public float rotationSpeed = 5f;
	}
}
