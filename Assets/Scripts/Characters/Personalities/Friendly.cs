using UnityEngine;

namespace DD
{
	[CreateAssetMenu(fileName = "New Friendly Personality")]
	public class Friendly : Personality
	{
		public Friendly(string personalityName, int maxTokens, float temperature) : base(personalityName, maxTokens, temperature) {}

		public override void Apply(NPC npc)
		{
			npc.personality = this;
		}
	}
}
