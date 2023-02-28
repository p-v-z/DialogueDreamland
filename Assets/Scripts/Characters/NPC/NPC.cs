using Sirenix.OdinInspector;
using UnityEngine;
using DD.UI;
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
			var initHandler = personality.Model.InstantiateAsync(graphicsParent);
			await initHandler.Task;

			var animatorController = initHandler.Result.GetComponentInChildren<Animator>();
			animatorController.runtimeAnimatorController = controller;
			animatorController.avatar = avatar;
		}
		
		public void StartDialogue(PlayerController player)
		{
			Debug.Log("Start dialogue");
			personality.Apply(this);
			
			// Look at each other
			transform.LookAt(player.transform);
			player.transform.LookAt(transform);
			
			// Show NPC intro
			DialogueManager.Instance.StartDialogue(this, HandleIntro);;
		}

		public void HandleIntro(string npcIntro)
		{
			DialogueManager.Instance.AddToConversation(new Conversation(null, npcIntro));
			
			Debug.Log($"Handle intro\n{npcIntro}");
			// Show the intro text
			GameUI.Instance.SetChatHistoryActive(true);

			// Show player text input
		}
	}
}
