using System.Runtime.InteropServices;
using UnityEngine;

public class InterOp : MonoBehaviour
{
    public void Test(string input)
    {
        Debug.Log("Test input");
        Debug.Log(input);
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