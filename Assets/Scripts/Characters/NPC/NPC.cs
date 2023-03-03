using Sirenix.OdinInspector;
using UnityEngine;
using DD.UI;
using Lightbug.CharacterControllerPro.Demo;
using UnityEditor.Animations;

namespace DD
{
	public class NPC : Character
	{
		[SerializeField, Required] public Personality personality;
		[SerializeField, Required] private Transform graphicsParent;
		[SerializeField, Required] private AnimatorController controller;
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
		}
		
		public void StartDialogue(PlayerController player)
		{
			
			// Disable CharacterControllerPro's movement
			var normalMovement = GetComponentInChildren<NormalMovement>();
			normalMovement.enabled = false;
			
			Debug.Log("Start dialogue");
			personality.Apply(this);
			
			// Look at each other
			transform.LookAt(player.transform);
			player.transform.LookAt(transform);
			
			// Show NPC intro
			DialogueManager.Instance.StartDialogue(this, HandleIntroResponse);;
		}

		private static void HandleIntroResponse(string npcIntro)
		{
			Debug.Log($"Handle intro\n{npcIntro}");
			DialogueManager.Instance.AddToConversation(new Conversation(null, npcIntro));
			GameUI.Instance.SetChatHistoryActive(true);
		}
	}
}
