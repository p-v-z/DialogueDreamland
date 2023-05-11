using UnityEngine;

namespace DD.WebGl
{
	/// <summary>
	/// This class is a wrapper for the InterOp classes that handles io communication between Unity and the browser JS.
	/// It acts as an interface, and a single point of entry for all communication - the InterOp game object.
	/// See <see cref="InterOpUnityToJs"/> and <see cref="InterOpJsToUnity"/> for more details.
	/// </summary>
    public class InterOp : MonoBehaviour
    {
#region Browser JS to Unity
	    public void SubmitApiKey(string key) => InterOpJsToUnity.SubmitApiKey(key);
	    public void SubmitChatMessage(string message) => InterOpJsToUnity.SubmitChatMessage(message);
	    public void SetMusicActive(bool active) => InterOpJsToUnity.SetMusicActive(active);
#endregion


#region Unity to Browser JS
	    public static void AddChatMessage(string message, bool fromPlayer = false) => InterOpUnityToJs.AddChatMessage(message, fromPlayer);
	    public static void SetInputActive(bool active) => InterOpUnityToJs.SetInputActive(active);
	    public static void SetChatActive(bool active) => InterOpUnityToJs.SetChatActive(active);
#endregion
    }
}
