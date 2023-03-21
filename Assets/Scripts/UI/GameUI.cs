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

        public static void SetItemActive<T>(T item, bool active) where T : VisualElement
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
    }
}