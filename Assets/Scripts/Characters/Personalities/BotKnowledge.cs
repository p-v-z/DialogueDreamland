using Sirenix.OdinInspector;
using UnityEngine;

namespace DD
{
	[CreateAssetMenu(fileName = "Bot Knowledge")]
	public class BotKnowledge : SerializedScriptableObject
	{
		public string Instructions { get => botInstructions; set => botInstructions = value; }
		[SerializeField] private string botInstructions;
	}
}
