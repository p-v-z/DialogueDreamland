using UnityEngine;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace DD.WebGl
{
	/// <summary>
	/// This class handles io communication from Unity to the browser JS.
	/// </summary>
	public class InterOpUnityToJs : MonoBehaviour
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		private void Awake()
		{
			// Disable keyboard input capture by default
			WebGLInput.captureAllKeyboardInput = false;
		}

		[DllImport("__Internal")] private static extern void AddChatJS(string msg, bool player); 
		[DllImport("__Internal")] private static extern void ShowInputJS(bool active);
		[DllImport("__Internal")] private static extern void SetChatActiveJS(bool active);

		/// <summary>
		/// Add a chat message to be rendered in browser HTML
		/// </summary>
		public static void AddChatMessage(string message, bool fromPlayer = false)
		{
			Debug.Log($"Calling JS from Unity, input: {message}");
			AddChatJS(message, fromPlayer);
		}
        
		/// <summary>
		/// Set the browser HTML input to be active or not
		/// </summary>
		public static void SetInputActive(bool active)
		{
			WebGLInput.captureAllKeyboardInput = !active;
			ShowInputJS(active);
		}

		/// <summary>
		/// Set the browser HTML input to be active or not
		/// </summary>
		public static void SetChatActive(bool active)
		{
			SetChatActiveJS(active);
		}
#else
		public static void AddChatMessage(string message, bool fromPlayer = false) => LogWebGL("AddChatMessage");
		public static void SetInputActive(bool active) => LogWebGL("SetInputActive");
		public static void SetChatActive(bool active) => LogWebGL("SetChatActive");

		private static void LogWebGL(string message) => Debug.Log($"WebGL IO function called: {message}");
#endif
	}
}
