using UnityEngine;

namespace DD.WebGl
{
	/// <summary>
	/// This class handles io communication from the browser JS to Unity.
	/// </summary>
	public class InterOpJsToUnity : MonoBehaviour
	{
		/// <summary>
		/// Submit a chat message that the player entered into the HTML input
		/// </summary>
		public static void SubmitApiKey(string key)
		{
			// Store the key
			PlayerPrefs.SetString("API_KEY", key);
			// Enable player
			PlayerController.Instance.SetMovementEnabled(true);
			Debug.Log("Set Unity api key from HTML input 🗝️");
			WebGLInput.captureAllKeyboardInput = true;
		}

		/// <summary>
		/// Submit a chat message that the player entered into the HTML input
		/// </summary>
		public static void SubmitChatMessage(string message)
		{
			Debug.Log("Unity handle chat submit from browser");
			DialogueManager.Instance.SaySomething(message);
		}

		/// <summary>
		/// Sets the looped background music to be active or not
		/// </summary>
		public static void SetMusicActive(bool active) => Camera.main.GetComponent<AudioSource>().mute = !active;
	}
}
