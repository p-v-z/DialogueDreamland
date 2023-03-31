using DD.WebGl;
using UnityEngine;
using UnityEngine.UIElements;

namespace DD.UI
{
    public class API_UI : Singleton<API_UI>
    {
        public bool Ready => currentKey != null && ValidateKey(currentKey);
        public void ClearText() => txtKey.Clear();

        private Button btnSet, btnGetKey;
        private TextField txtKey;
        private string currentKey;
        
        private bool Log { get; } = false;

        private void OnEnable()
        {
            var rootDoc = GetComponent<UIDocument>();
            var root = rootDoc.rootVisualElement;
            btnSet = root.Q<Button>("BtnSet");
            txtKey = root.Q<TextField>("TxtKeyAPI");
            
            // On key value change
            txtKey.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (Log) Debug.Log("Key changed");
                var valid = ValidateKey(evt.newValue);
                btnSet.SetEnabled(valid);
                
                txtKey.RemoveFromClassList("valid");
                txtKey.RemoveFromClassList("invalid");
                if (evt.newValue != "")
                {
                    txtKey.AddToClassList(valid ? "valid" : "invalid");
                }

                if (valid)
                {
                    btnSet.Focus();
                }
                    
                GameUI.SetItemActive(btnGetKey, !valid);
            });
            
            btnSet.RegisterCallback<ClickEvent>(HandleSet);
            btnSet.SetEnabled(false);
            
            btnGetKey = root.Q<Button>("BtnGetKey");
            btnGetKey.RegisterCallback<ClickEvent>(evt =>
            {
                Application.OpenURL("https://platform.openai.com/account/api-keys");
            });
            
            // Check if the API key is already set
            currentKey = PlayerPrefs.GetString("API_KEY");
            if (!ValidateKey(currentKey))
            {
                gameObject.SetActive(true);
                GameUI.Instance.SetClearBtnActive(false);
                currentKey = null;
            }
            else
            {
                SetAPIKey(currentKey);
            }

            // InterOp.OnPaste += HandlePaste;
        }
        
        private static bool ValidateKey(string apiKey)
        {
            var valid = false;
            var correctLength = apiKey.Length == 51;
            var isNotEmpty = !string.IsNullOrEmpty(apiKey);
            if (isNotEmpty && correctLength)
            {
                var isValidSub = apiKey[..3] == "sk-";
                valid = isValidSub;
            }
            return valid;
        }

        private void OnDisable()
        {
            // InterOp.OnPaste -= HandlePaste;
            btnSet.UnregisterCallback<ClickEvent>(HandleSet);
        }

        private void HandlePaste(string clipboard)
        {
            if (Log) Debug.Log("Pasted clipboard from browser");
            txtKey.value = clipboard;
        }
        
        private void HandleSet(ClickEvent evt)
        {
            SetAPIKey();
        }
        
        void SetAPIKey(string providedKey = "")
        {
            if (Log) Debug.Log("Handle set API key");
            // TODO: Validate input
            var hasProvidedKey = !string.IsNullOrEmpty(providedKey);
            var key =  hasProvidedKey ? providedKey : txtKey.value;
            PlayerPrefs.SetString("API_KEY", key);

            // Hide the UI
            gameObject.SetActive(false);
            GameUI.Instance.SetClearBtnActive(true);
            
            // Enable player
            PlayerController.Instance.SetMovementEnabled(true);
        }
    }
}