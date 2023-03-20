using Sirenix.OdinInspector;
using UnityEngine;
using Lightbug.CharacterControllerPro.Demo;

namespace DD
{
	public class NPC : Character
	{
		[SerializeField, Required] public Personality personality;
		[SerializeField, Required] private Transform graphicsParent;
		[SerializeField, Required] private RuntimeAnimatorController controller;
		[SerializeField, Required] private Avatar avatar;

		private async void Start()
		{
			// Instantiate the NPC model
			var initHandler = personality.Model.InstantiateAsync(graphicsParent);
			await initHandler.Task;

			// Setup avatar and animator
			var animatorController = initHandler.Result.GetComponentInChildren<Animator>();
			animatorController.runtimeAnimatorController = controller;
			animatorController.avatar = avatar;
			
			personality.AddNameToPrompt();
		}
		
		public void StartDialogue(PlayerController player)
		{
			// Disable CharacterControllerPro's movement
			var normalMovement = GetComponentInChildren<NormalMovement>();
			normalMovement.enabled = false;
			personality.Apply(this);
			
			// Look at each other
			transform.LookAt(player.transform);
			player.transform.LookAt(transform);
			
			// Show NPC intro
			DialogueManager.Instance.StartDialogue(this, HandleIntroResponse);;
		}

		private static void HandleIntroResponse(string npcIntro)
		{
			DialogueManager.Instance.AddToConversation(new Conversation(null, npcIntro));
		}
	}
}
