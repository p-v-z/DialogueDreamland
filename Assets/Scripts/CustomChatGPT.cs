using ChatGPTWrapper;

namespace DD.API
{
	public class CustomChatGPT : ChatGPTConversation
	{
		public void Setup(string apiKey, string initialPrompt)
		{
			SetApiKey(apiKey);
			ResetChat(initialPrompt);
		}
	}
}
