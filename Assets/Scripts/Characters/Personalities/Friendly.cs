using UnityEngine;

namespace DD
{
	[CreateAssetMenu(fileName = "New Friendly Personality")]
	public class Friendly : Personality
	{
		public Friendly(string personalityName)
		{
			PersonalityName = personalityName;
		}

		public override void Apply(NPC npc)
		{
			npc.personality = this;
		}
	}
}
