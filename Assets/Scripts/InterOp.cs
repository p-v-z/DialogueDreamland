using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DD.WebGl
{
    public class InterOp : MonoBehaviour
    {
        public static Action<string> OnPaste = delegate {};

        // JS -> Unity
        public void Paste(string clipboard)
        {
            Debug.Log("Unity handle paste from browser, input: " + clipboard);
            OnPaste.Invoke(clipboard);
        }
        
        // Unity -> JS
        [DllImport("__Internal")] private static extern void AddChatJS(string msg, bool player); 
        public static void AddChatMessage(string message, bool fromPlayer = false)
        {
            Debug.Log($"Testing JS from Unity, input: {message}");
#if UNITY_WEBGL && !UNITY_EDITOR
            // Call JavaScript function using WebGL API
            AddChatJS(message, fromPlayer);
#else
            Debug.Log("Browser JS function called from Unity (Not currently in browser)");
#endif
        }
        
        [DllImport("__Internal")] private static extern void ShowInputJS(bool active);
        public static void SetInputActive(bool active)
        {
	        WebGLInput.captureAllKeyboardInput = !active;
	        ShowInputJS(active);
        }
    }
}