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

        public string PersonalityName { get => personalityName; private set => personalityName = value; }
        [SerializeField] private string personalityName;
        
        public float Temperature { get => temperature; private set => temperature = value; }
        [SerializeField] private float temperature;
        
        public int MaxTokens { get => maxTokens; private set => maxTokens = value; }
        [SerializeField] private int maxTokens;
        
        public abstract void Apply(NPC npc);
    }
}