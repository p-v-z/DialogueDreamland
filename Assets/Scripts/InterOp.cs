using System;
using UnityEngine;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace DD.WebGl
{
	/// <summary>
	/// This class handles communication between Unity and the browser
	/// </summary>
    public class InterOp : MonoBehaviour
    {
		void Awake()
		{
			// Disable keyboard input capture by default
			WebGLInput.captureAllKeyboardInput = false;
		}

#region JS -> Unity
	    /// <summary>
	    /// Submit a chat message that the player entered into the HTML input
	    /// </summary>
	    public void SubmitApiKey(string key)
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
	    public void SubmitChatMessage(string message)
	    {
		    Debug.Log("Unity handle chat submit from browser");
		    DialogueManager.Instance.SaySomething(message);
		    AddChatMessage(message, true);
	    }
#endregion

	    
#region Unity -> JS
	#if UNITY_WEBGL && !UNITY_EDITOR
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

	    private static void LogWebGL(string message) => Debug.Log($"WebGL IO function called: {message}");
	#endif
#endregion
    }
}