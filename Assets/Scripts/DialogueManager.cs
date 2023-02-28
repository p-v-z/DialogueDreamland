using System;
using System.Collections;
using System.Collections.Generic;
using ChatGPTWrapper;
using DD.API;
using DD.UI;
using UnityEngine;
using UnityEngine.Events;

namespace DD
{
	public class DialogueManager : Singleton<DialogueManager>
	{
		[SerializeField] ChatGPTConversation chatGPTConversation;
		
		public UnityEvent OnDialogueStarted = new UnityEvent();
		public UnityEvent OnDialogueEnded = new UnityEvent();

		// Conversation history with the player
		private readonly List<Conversation> conversationHistory = new ();
		public void AddToConversation(Conversation conv) => conversationHistory.Add(conv);

		protected override void Awake()
		{
			base.Awake();
			chatGPTConversation.enabled = false;
		}
		
		public void StartDialogue(NPC npc, Action<string> introHandler)
		{
			// Trigger the OnDialogueStarted event
			OnDialogueStarted.Invoke();

			var apiKey = PlayerPrefs.GetString("API_KEY");
			var botName = npc.personality.PersonalityName;
			var primer = npc.personality.PrimerPrompt;
			
			// Validate that api key, bot name, and primer are not empty
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(botName) || string.IsNullOrEmpty(primer))
			{
				Debug.LogError("API key, bot name, or primer prompt is empty");
				return;
			}
			
			chatGPTConversation.SetupGPT(apiKey, botName, primer, npc.personality.MaxTokens, npc.personality.Temperature);

			chatGPTConversation.chatGPTResponse.AddListener((string response) =>
			{
				Debug.Log($"ChatGPT response: {response}");
				GameUI.Instance.AddChatHistoryItem(false, response);
				GameUI.Instance.SetChatInputActive(true);
				introHandler(response);
			});
			StartCoroutine(ChatRoutine(primer));
		}

		private IEnumerator ChatRoutine(string primer)
		{
			chatGPTConversation.enabled = true;
			yield return new WaitForSeconds(1f);
			chatGPTConversation.SendToChatGPT(primer);
		}
		
		private void HandleChatResponse(string response)
		{
			Debug.Log($"ChatGPT response: {response}");
		}
	}
}
