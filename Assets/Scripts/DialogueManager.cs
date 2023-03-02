using System;
using System.Collections;
using System.Collections.Generic;
using DD.API;
using DD.UI;
using UnityEngine;
using UnityEngine.Events;

namespace DD
{
	public class DialogueManager : Singleton<DialogueManager>
	{
		[SerializeField] CustomChatGPT chatGPTConversation;
		
		public UnityEvent OnDialogueStarted = new UnityEvent();
		public UnityEvent OnDialogueEnded = new UnityEvent();

		// Conversation history with the player
		public void AddToConversation(Conversation conv) => conversationHistory.Add(conv);
		private readonly List<Conversation> conversationHistory = new ();
		
		private NPC currentNPC;
		private List<Conversation> currentConversation;
		private readonly Dictionary<NPC, List<Conversation>> npcConversations = new ();

		private string lastPlayerMessage;


		private Action<string> introHandler;

		protected override void Awake()
		{
			base.Awake();
			chatGPTConversation.enabled = false;
		}
		
		/// <summary>
		/// This is called when the player starts a conversation with an NPC
		/// </summary>
		public void StartDialogue(NPC npc, Action<string> introHandler)
		{
			this.introHandler = introHandler;
			
			// Set the current NPC
			currentNPC = npc;

			// Trigger the OnDialogueStarted event
			OnDialogueStarted.Invoke();
			GameUI.Instance.SetTalkBtnActive(false);
			
			// TODO: Show loading icon

			// Validate that api key, bot name, and primer are not empty
			var apiKey = PlayerPrefs.GetString("API_KEY");
			var botName = npc.personality.PersonalityName;
			var primer = npc.personality.PrimerPrompt;
			if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(botName) || string.IsNullOrEmpty(primer))
			{
				Debug.LogError("API key, bot name, or primer prompt is empty");
				return;
			}

			Debug.Log("Setting up ChatGPT");
			chatGPTConversation.SetupGPT(apiKey, botName, primer, npc.personality.MaxTokens, npc.personality.Temperature);
			chatGPTConversation.chatGPTResponse.AddListener(HandleChatResponse);
			StartCoroutine(ChatRoutine(primer));
		}
		
		public void EndDialogue()
		{
			// Trigger the OnDialogueEnded event
			OnDialogueEnded.Invoke();
			GameUI.Instance.SetTalkBtnActive(true);
			GameUI.Instance.SetChatHistoryActive(false);
			GameUI.Instance.SetChatInputActive(false);
			
			currentNPC = null;
			currentConversation = null;
			chatGPTConversation.chatGPTResponse.RemoveListener(HandleChatResponse);
		}

		/// <summary>
		/// ChatGPTConversation is disabled by default, so we need to enable it and give it a chance to assign it's values
		/// before sending the primer prompt
		/// </summary>
		private IEnumerator ChatRoutine(string primer)
		{
			Debug.Log("Enabling ChatGPT");
			chatGPTConversation.enabled = true;
			yield return new WaitUntil(() => chatGPTConversation.Ready);
			
			Debug.Log("Sending primer prompt to ChatGPT");
			chatGPTConversation.SendToChatGPT(primer);
		}

		/// <summary>
		/// Handles the response from ChatGPT
		/// </summary>
		private void HandleChatResponse(string response)
		{
			Debug.Log($"ChatGPT response: {response}");
			
			// Make sure we are still in the conversation
			if (currentNPC == null)
			{
				Debug.LogWarning("Chat response when current NPC is null - player probably left the conversation");
				return;
			}
			
			// Add the response to the conversation history
			var converse = new Conversation(lastPlayerMessage, response);
			if (npcConversations.ContainsKey(currentNPC))
			{
				Debug.Log($"Added conversation to history: {converse.playerMessage} - {converse.npcResponse}");	
				currentConversation = npcConversations[currentNPC];
				currentConversation.Add(converse);
			}
			else
			{
				Debug.Log("Adding new conversation to history");
				var newConversation = new List<Conversation> {converse};
				npcConversations.Add(currentNPC,  newConversation);
				currentConversation = newConversation;
			}

			// Update UI
			GameUI.Instance.AddChatHistoryItem(false, response);
			GameUI.Instance.SetChatInputActive(true);
			
			// Invoke the intro handler
			if (introHandler != null)
			{
				introHandler.Invoke(response);
				introHandler = null;
			}
		}
		
		/// <summary>
		/// Handles chat input from the player
		/// </summary>
		public void SaySomething(string playerInput)
		{
			lastPlayerMessage = playerInput;
			chatGPTConversation.SendToChatGPT(playerInput);
		}
	}
}
