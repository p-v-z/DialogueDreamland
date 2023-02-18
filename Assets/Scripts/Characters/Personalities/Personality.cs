using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DD
{
    public interface IPersonality
    {
        string PersonalityName { get; set; }
        float Temperature { get; }
        int MaxTokens { get; }
        string PrimerPrompt { get; }
        
        void Apply(NPC npc);
    }
    
    public abstract class Personality : SerializedScriptableObject, IPersonality
    {
        /// <summary>
        /// Unique name of the personality.
        /// </summary>
        public string PersonalityName { get => personalityName; set => personalityName = value; }
        [SerializeField] private string personalityName;

        /// <summary>
        /// Randomness of the generated response.
        /// </summary>
        public float Temperature { get => temperature; protected set => temperature = value; }
        [SerializeField, Range(0, 1)] private float temperature;

        /// <summary>
        /// Optional parameter used in the OpenAI GPT API to specify the maximum number of tokens that the API should generate as output.
        /// </summary>
        public int MaxTokens { get => maxTokens; protected set => maxTokens = value; }
        [SerializeField, Range(1, 2048)] private int maxTokens;

        /// <summary>
        /// The prompt that the API will use to base the character's personality on.
        /// </summary>
        public string PrimerPrompt { get => primerPrompt; protected set => primerPrompt = value; }
        [SerializeField, TextArea(3, 20)] private string primerPrompt;

        /// <summary>
        /// This is the reference to the 3D model to be used for the character
        /// </summary>
        public AssetReference Model { get => model; protected set => model = value; }
        [SerializeField, AssetsOnly] private AssetReference model; 
        
        public abstract void Apply(NPC npc);
    }
}