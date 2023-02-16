using UnityEngine;

namespace DD
{
    [CreateAssetMenu(fileName = "New Personality")]
    public abstract class Personality : ScriptableObject, IPersonality
    {
        protected Personality(string personalityName, int maxTokens, float temperature)
        {
            PersonalityName = personalityName;
            MaxTokens = maxTokens;
            Temperature = temperature;
        }

        public string PersonalityName { get; }
        public int MaxTokens { get; }
        public float Temperature { get;}
        public abstract void Apply(NPC npc);
    }
}