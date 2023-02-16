namespace DD
{
	[System.Serializable]
	public class Conversation
	{
		// The message sent by the player
		public string playerMessage;

		// The response from the NPC
		public string npcResponse;

		public Conversation(string playerMessage, string npcResponse)
		{
			this.playerMessage = playerMessage;
			this.npcResponse = npcResponse;
		}
	}
}
