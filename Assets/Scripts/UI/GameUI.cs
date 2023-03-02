using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DD.UI
{
    public class GameUI : Singleton<GameUI>
    {
        [SerializeField] private VisualTreeAsset chatDialogue;
        
        public Button btnTalk { get; private set; }
        
        private Button btnSay;

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

            btnSay = root.Q<Button>("BtnSay");
            btnSay.RegisterCallback<ClickEvent>(HandleSay);
            txtChatInput = root.Q<TextField>("TxtInput");
            txtChatInput.RegisterCallback<KeyDownEvent>(HandleKeyDown);

            grpChatHistory = root.Q<GroupBox>("ChatHistory");
            grpChatInput = root.Q<GroupBox>("ChatInput");
            
            // SetItemsActive(new List<VisualElement> {btnTalk, btnSay, grpChatHistory, grpChatInput}, false);
            SetTalkBtnActive(false);
            SetChatHistoryActive(false);
            SetChatInputActive(false);
        }
        
        private void HandleKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return)
            {
                Debug.Log("Enter pressed ");
            }
        }

        private void HandleSay(ClickEvent evt)
        {
            Debug.Log($"Say: {evt.target}");
            
            SetChatInputActive(false);
            AddChatHistoryItem(true, txtChatInput.value);
            DialogueManager.Instance.SaySomething(txtChatInput.value);
            
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
        
        private void SetItemsActive<T>(IEnumerable<T> items, bool active) where T : VisualElement
        {
            foreach (var item in items)
            {
                SetItemActive(item, active);
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