using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DD
{
    public interface IPersonality
    {
        string PersonalityName { get; set; }
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
        /// The prompt that the API will use to base the character's personality on.
        /// </summary>
        public string PrimerPrompt { get => primerPrompt; protected set => primerPrompt = value; }
        [SerializeField, TextArea(3, 20)] private string primerPrompt;

        /// <summary>
        /// This is the reference to the 3D model to be used for the character
        /// </summary>
        public AssetReference Model { get => model; protected set => model = value; }
        [SerializeField] private AssetReference model; 
        
        public abstract void Apply(NPC npc);
        
        public void AddNameToPrompt()
        {
            PrimerPrompt = PrimerPrompt.Replace("{NAME}", personalityName);
        }
    }
}