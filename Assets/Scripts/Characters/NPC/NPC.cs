using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DD
{
	public class NPC : Character
	{
		[SerializeField, Required] public Personality personality;
		[SerializeField, Required] private CConfig config;
		
		// Conversation history with the player
		private List<Conversation> conversationHistory = new List<Conversation>();

		private async void Start()
		{
			var initHandler = personality.Model.InstantiateAsync(transform);
			await initHandler.Task;

			var animatorController = initHandler.Result.GetComponentInChildren<Animator>();
			animatorController.runtimeAnimatorController = config.animatorController;
		}
		
		public void StartDialogue()
		{
			Debug.Log("Start dialogue");
			personality.Apply(this);
		}
	}
}
