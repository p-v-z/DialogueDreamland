using UnityEngine;
using UnityEngine.UIElements;

namespace DD.UI
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private VisualTreeAsset chatDialogue;
        
        public Button btnTalk { get; private set; }
        
        private Button btnSay, btnClear;

        private GroupBox grpChatHistory;
        private GroupBox grpChatInput;
        
        private TextField txtChatInput;

        private UIDocument rootDoc;
        private VisualElement root;

        protected override void Awake()
        {
            base.Awake();
            
            rootDoc = GetComponent<UIDocument>();
            root = rootDoc.rootVisualElement;

            btnTalk = root.Q<Button>("BtnTalk");
            btnTalk.RegisterCallback<ClickEvent>(HandleTalk);
            btnClear = root.Q<Button>("BtnClearCache");
            btnClear.RegisterCallback<ClickEvent>(HandleClear);

            btnSay = root.Q<Button>("BtnSay");
            btnSay.RegisterCallback<ClickEvent>(HandleSay);
            txtChatInput = root.Q<TextField>("TxtInput");
            txtChatInput.RegisterCallback<KeyDownEvent>(HandleKeyDown);

            grpChatHistory = root.Q<GroupBox>("ChatHistory");
            grpChatInput = root.Q<GroupBox>("ChatInput");
            
            SetTalkBtnActive(false);
            SetChatHistoryActive(false);
            SetChatInputActive(false);
        }
        
        private void HandleClear(ClickEvent evt)
        {
            PlayerPrefs.DeleteKey("API_KEY");
            var keyUI = FindAnyObjectByType<API_UI>(FindObjectsInactive.Include);
            keyUI.ClearText();
            keyUI.gameObject.SetActive(true);
        }

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
            Debug.Log($"Speaking: {currentInput}");
            
            // Add to chat history and send to DialogueManager
            AddChatHistoryItem(true, currentInput);
            DialogueManager.Instance.SaySomething(currentInput);
            
            // Clear input and disable
            txtChatInput.value = "";
            SetChatInputActive(false);
        }

        private void HandleTalk(ClickEvent evt)
        {
            Debug.Log($"Talk: {evt.target}");
        }

        public void SetTalkBtnActive(bool active) => SetItemActive(btnTalk, active);
        public void SetChatHistoryActive(bool active) => SetItemActive(grpChatHistory, active);
        public void SetChatInputActive(bool active) => SetItemActive(grpChatInput, active);

        private void SetItemActive<T>(T item, bool active) where T : VisualElement
        {
            if (active)
            {
                item.RemoveFromClassList("hidden");
            }
            else
            {
                item.AddToClassList("hidden");
            }
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
    }
}