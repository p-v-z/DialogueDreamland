using ChatGPTWrapper;

namespace DD.API
{
	public class CustomChatGPT : ChatGPTConversation
	{
		// API endpoint and access token
		public bool Ready => _selectedModel != null; 
		public void SetupGPT(string apiKey, string chatBotName, string initialPrompt, int maxTokens, float temperature)
		{
			_apiKey = apiKey;
			_chatbotName = chatBotName;
			_initialPrompt = initialPrompt;
			_maxTokens = maxTokens;
			_temperature = temperature;
		}
	}
}
