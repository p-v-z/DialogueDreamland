using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DD.WebGl
{
    public class InterOp : MonoBehaviour
    {
        public static Action<string> OnPaste = delegate {};

        public void Paste(string clipboard)
        {
            Debug.Log("Unity handle paste from browser, input: " + clipboard);
            OnPaste.Invoke(clipboard);
        }
    }

    public static class Clipboard
    {
        [DllImport("__Internal")]
        private static extern void CopyToClipboard(string text);

        public static void SetText(string text)
        {
#if UNITY_WEBGL && UNITY_EDITOR == false
            CopyToClipboard(text);
#else
            GUIUtility.systemCopyBuffer = text;
#endif
        }
    }
}