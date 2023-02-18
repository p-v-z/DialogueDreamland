using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DD.API
{
	public static class API
	{
		// API endpoint and access token
		private const string API_ENDPOINT = "https://api.openai.com/v1/engines/davinci-codex/completions";
		private const string API_KEY = "YOUR_API_KEY_HERE";
		
		// Sends a message to the NPC and returns their response
		public static IEnumerator SendMessage(string message, IPersonality personality, Action<string> callback, List<Conversation> conversationHistory)
		{
			// Build the request
			var request = UnityWebRequest.PostWwwForm(API_ENDPOINT, "");
			request.SetRequestHeader("Authorization", $"Bearer {API_KEY}");
			request.SetRequestHeader("Content-Type", "application/json");

			// Build the JSON body
			var jsonBody = BuildJsonBody(message, personality);

			// Send the request
			var bodyRaw = new System.Text.UTF8Encoding(true).GetBytes(jsonBody);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
			yield return request.SendWebRequest();

			// Check for errors
			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(request.error);
				yield break;
			}

			// Parse the response and get the generated text
			var responseJson = request.downloadHandler.text;
			var generatedText = ParseJsonResponse(responseJson);

			// Add the conversation to the history
			conversationHistory.Add(new Conversation(message, generatedText));

			// Return the generated text to the callback
			callback?.Invoke(generatedText);
		}

		// Builds the JSON body for the OpenAI GPT API request
		private static string BuildJsonBody(string message, IPersonality person)
		{
			var jsonBody = "{\"prompt\": \"" + message + "\", \"max_tokens\": " + person.MaxTokens + ", \"temperature\": " + person.Temperature + "}";
			return jsonBody;
		}

		// Parses the JSON response from the OpenAI GPT API and returns the generated text
		private static string ParseJsonResponse(string jsonResponse)
		{
			// Parse the response JSON
			var json = JObject.Parse(jsonResponse);

			// Get the generated text from the response
			var text = (string)json["choices"][0]["text"];

			// Return the generated text
			return text;
		}
	}
}
