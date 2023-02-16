namespace DD
{
	public interface IPersonality
	{
		/// <summary>
		/// Unique name of the personality.
		/// </summary>
		string PersonalityName { get; }
        
		/// <summary>
		/// randomness of the generated response.
		/// </summary>
		float Temperature { get; }
        
		/// <summary>
		/// Range(1, 2048). Optional parameter used in the OpenAI GPT API to specify the maximum number of tokens that the API should generate as output.
		/// </summary>
		int MaxTokens { get; }
        
		void Apply(NPC npc);
	}
}
