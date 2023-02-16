using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DD
{
	public class NPC : Character
	{
		public NPCBehavior behavior;

		public NPCDialogue dialogue;

		public Personality personality;

		public void StartDialogue()
		{
			personality.Apply(this);
			dialogue.StartDialogue();
		}

		public void UpdateBehavior()
		{
			behavior.UpdateBehavior();
		}

		// Unique identifier for this NPC
		public string id;

		// Conversation history with the player
		private List<Conversation> conversationHistory = new List<Conversation>();

		// API endpoint and access token
		private const string API_ENDPOINT = "https://api.openai.com/v1/engines/davinci-codex/completions";
		private const string API_KEY = "YOUR_API_KEY_HERE";

		// Sends a message to the NPC and returns their response
		public IEnumerator SendMessage(string message, System.Action<string> callback)
		{
			// Build the request
			UnityWebRequest request = UnityWebRequest.PostWwwForm(API_ENDPOINT, "");
			request.SetRequestHeader("Authorization", $"Bearer {API_KEY}");
			request.SetRequestHeader("Content-Type", "application/json");

			// Build the JSON body
			string jsonBody = BuildJsonBody(message, personality);

			// Send the request
			byte[] bodyRaw = new System.Text.UTF8Encoding(true).GetBytes(jsonBody);
			request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			yield return request.SendWebRequest();

			// Check for errors
			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(request.error);
				yield break;
			}

			// Parse the response and get the generated text
			string responseJson = request.downloadHandler.text;
			string generatedText = ParseJsonResponse(responseJson);

			// Add the conversation to the history
			conversationHistory.Add(new Conversation(message, generatedText));

			// Return the generated text to the callback
			callback?.Invoke(generatedText);
		}

		// Builds the JSON body for the OpenAI GPT API request
		private string BuildJsonBody(string message, IPersonality person)
		{
			string jsonBody = "{\"prompt\": \"" + message + "\", \"max_tokens\": " + person.MaxTokens + ", \"temperature\": " + person.Temperature + "}";
			return jsonBody;
		}

		// Parses the JSON response from the OpenAI GPT API and returns the generated text
		private string ParseJsonResponse(string jsonResponse)
		{
			// Parse the response JSON
			JObject json = JObject.Parse(jsonResponse);

			// Get the generated text from the response
			string text = (string)json["choices"][0]["text"];

			// Return the generated text
			return text;
		}
	}
}
