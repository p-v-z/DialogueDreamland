using UnityEngine;
using UnityEngine.UIElements;

namespace DD.UI
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private VisualTreeAsset chatDialogue;
        
        public Button btnTalk { get; private set; }
        
        private Button btnClear;
        private UIDocument rootDoc;
        private VisualElement root;
        private GroupBox grpChatInput;
        
#if UNITY_EDITOR
        private GroupBox grpChatHistory;
        private TextField txtChatInput;
#endif
        
        private bool Log { get; } = false;
        
        protected override void Awake()
        {
            base.Awake();
            
            rootDoc = GetComponent<UIDocument>();
            root = rootDoc.rootVisualElement;

            btnTalk = root.Q<Button>("BtnTalk");
            btnTalk.RegisterCallback<ClickEvent>(HandleTalk);
            btnClear = root.Q<Button>("BtnClearCache");
            btnClear.RegisterCallback<ClickEvent>(HandleClear);
            
            SetTalkBtnActive(false);
            SetClearBtnActive(false);
            
            grpChatInput = root.Q<GroupBox>("ChatInput");
#if UNITY_EDITOR
            txtChatInput = root.Q<TextField>("TxtInput");
            txtChatInput.RegisterCallback<KeyDownEvent>(HandleKeyDown);
            grpChatHistory = root.Q<GroupBox>("ChatHistory");
#endif
            SetChatInputActive(false);
        }
        
        private void HandleClear(ClickEvent evt)
        {
            PlayerPrefs.DeleteKey("API_KEY");
            var keyUI = FindObjectOfType<API_UI>(true);
            keyUI.ClearText();
            keyUI.gameObject.SetActive(true);
            SetClearBtnActive(false);
        }

        private void HandleTalk(ClickEvent evt)
        {
            if (Log) Debug.Log($"Talk: {evt.target}");
        }

        public void SetClearBtnActive(bool active) => SetItemActive(btnClear, active);
        public void SetTalkBtnActive(bool active) => SetItemActive(btnTalk, active);
        public void SetChatInputActive(bool active) => SetItemActive(grpChatInput, active);
        
        public static void SetItemActive<T>(T item, bool active) where T : VisualElement
        {
            if (item == null) return;
            
            if (active)
            {
                item.RemoveFromClassList("hidden");
            }
            else
            {
                item.AddToClassList("hidden");
            }
        }

#if UNITY_EDITOR
        public void SetChatHistoryActive(bool active) => SetItemActive(grpChatHistory, active);

        private void HandleKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return)
            {
                Speak();
            }
        }

        private void HandleSay(ClickEvent evt)
        {
            Speak();
        }

        private void Speak()
        {
            // Get input
            var currentInput = txtChatInput.value;
            if (Log) Debug.Log($"Speaking: {currentInput}");
            
            // Add to chat history and send to DialogueManager
            AddChatHistoryItem(true, currentInput);
            DialogueManager.Instance.SaySomething(currentInput);
            
            // Clear input and disable
            txtChatInput.value = "";
            SetChatInputActive(false);
        }
        
        public void AddChatHistoryItem(bool player, string text)
        {
            var item = chatDialogue.Instantiate();
            var messageText = item.Q<TextField>("TxtSaid");
            messageText.value = text;
            item.AddToClassList("chat-history-item");
            item.AddToClassList(player ? "user" : "ai");
            grpChatHistory.Add(item);
        }
#endif
    }
}