using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DD.UI
{
    public class GameUI : Singleton<GameUI>
    {
        public Button btnTalk { get; private set; }
        private Button btnSay;

        private GroupBox grpChatHistory;
        private GroupBox grpChatInput;

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

            grpChatHistory = root.Q<GroupBox>("ChatHistory");
            grpChatInput = root.Q<GroupBox>("ChatInput");
            
            // SetItemsActive(new List<VisualElement> {btnTalk, btnSay, grpChatHistory, grpChatInput}, false);
            SetTalkBtnActive(false);
            SetChatHistoryActive(false);
            SetChatInputActive(false);
        }

        private void HandleSay(ClickEvent evt)
        {
            Debug.Log($"Say: {evt.target}");
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
    }
}