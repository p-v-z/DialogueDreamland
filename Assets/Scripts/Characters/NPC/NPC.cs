using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DD.API;
using DD.UI;

namespace DD
{
	public class NPC : Character
	{
		[SerializeField, Required] public Personality personality;
		

		private async void Start()
		{
			var initHandler = personality.Model.InstantiateAsync(transform);
			await initHandler.Task;

			var animatorController = initHandler.Result.GetComponentInChildren<Animator>();
		}
		
		public void StartDialogue(PlayerController player)
		{
			Debug.Log("Start dialogue");
			personality.Apply(this);
			
			// Look at each other
			transform.LookAt(player.transform);
			player.transform.LookAt(transform);
			
			// Show NPC intro
			DialogueManager.Instance.StartDialogue(this, HandleIntro);;
		}

		public void HandleIntro(string npcIntro)
		{
			DialogueManager.Instance.AddToConversation(new Conversation(null, npcIntro));
			
			Debug.Log($"Handle intro\n{npcIntro}");
			// Show the intro text
			GameUI.Instance.SetChatHistoryActive(true);

			// Show player text input
		}
	}
}
