using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DD
{
	public class NPC : Character
	{
		public NPCBehavior behavior;
		public NPCDialogue dialogue;

		[SerializeField, Required] public Personality personality;
		
		// Conversation history with the player
		private List<Conversation> conversationHistory = new List<Conversation>();

		private void Start()
		{
			personality.Model.InstantiateAsync(transform);
		}
		
		public void StartDialogue()
		{
			personality.Apply(this);
			dialogue.StartDialogue();
		}

		public void UpdateBehavior()
		{
			behavior.UpdateBehavior();
		}
	}
}
