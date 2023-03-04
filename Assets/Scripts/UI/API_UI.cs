using DD.WebGl;
using UnityEngine;
using UnityEngine.UIElements;

namespace DD.UI
{
    public class API_UI : MonoBehaviour
    {
        public void ClearText() => txtKey.Clear();
        
        private Button btnSet;
        private TextField txtKey;
        

        private void OnEnable()
        {
            Debug.Log("Enable API UI");
            var rootDoc = GetComponent<UIDocument>();
            var root = rootDoc.rootVisualElement;
            btnSet = root.Q<Button>("BtnSet");
            txtKey = root.Q<TextField>("TxtKeyAPI");
            btnSet.RegisterCallback<ClickEvent>(HandleSet);
            
            // Check if the API key is already set
            var key = PlayerPrefs.GetString("API_KEY");
            if (!string.IsNullOrEmpty(key))
            {
                // Hide the UI
                gameObject.SetActive(false);
                GameUI.Instance.SetClearBtnActive(true);
            }

            InterOp.OnPaste += HandlePaste;
        }

        private void OnDisable()
        {
            InterOp.OnPaste -= HandlePaste;
            btnSet.UnregisterCallback<ClickEvent>(HandleSet);
        }

        private void HandlePaste(string clipboard)
        {
            Debug.Log("Pasted from browser: " + clipboard);
            txtKey.value = clipboard;
        }
        
        private void HandleSet(ClickEvent evt)
        {
            Debug.Log("Handle set");
            // TODO: Validate input

            var key = txtKey.value;
            PlayerPrefs.SetString("API_KEY", key);

            // Hide the UI
            // enabled = false;
            gameObject.SetActive(false);
            GameUI.Instance.SetClearBtnActive(true);
        }
    }
}