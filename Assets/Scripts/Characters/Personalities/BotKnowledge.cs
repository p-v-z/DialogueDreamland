using Sirenix.OdinInspector;
using UnityEngine;

namespace DD
{
	[CreateAssetMenu(fileName = "Bot Knowledge")]
	public class BotKnowledge : SerializedScriptableObject
	{
		public string Instructions { get => botInstructions; set => botInstructions = value; }
		[SerializeField] private string botInstructions;
		
		// Comes before the prompt
		public string PrePrompt { get => prePrompt; set => prePrompt = value; }
		[SerializeField] private string prePrompt;
		
		// Comes after the prompt
		public string PostPrompt { get => postPrompt; set => postPrompt = value; }
		[SerializeField] private string postPrompt;
		
		// Comes after the post prompt
		public string StartIntro { get => startIntro; set => startIntro = value; }
		[SerializeField] private string startIntro;
	}
}
