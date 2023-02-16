using UnityEngine;
using UnityEngine.Events;

namespace DD
{
	public class DialogueManager : MonoBehaviour
	{
		public UnityEvent OnDialogueStarted;
		public UnityEvent OnDialogueEnded;

		public void StartDialogue(NPC npc)
		{
			// Trigger the OnDialogueStarted event
			OnDialogueStarted.Invoke();

			// Start the dialogue with the given NPC
			npc.StartDialogue();

			// Trigger the OnDialogueEnded event
			OnDialogueEnded.Invoke();
		}
	}
}
