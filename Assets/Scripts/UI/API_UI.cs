using UnityEngine;
using UnityEngine.UIElements;

public class API_UI : MonoBehaviour
{
    private Button btnSet;

    private TextField txtKey;

    void Awake()
    {
        var rootDoc = GetComponent<UIDocument>();
        var root = rootDoc.rootVisualElement;
        
        btnSet = root.Q<Button>("BtnSet");
        btnSet.RegisterCallback<ClickEvent>(HandleSet);
        
        txtKey = root.Q<TextField>("TxtKeyAPI");
        
        // Check if the API key is already set
        var key = PlayerPrefs.GetString("API_KEY");
        if (!string.IsNullOrEmpty(key))
        {
            // Hide the UI
            gameObject.SetActive(false);
        }
    }

    public void ClearText()
    {
        txtKey.Clear();
    }
    
    private void HandleSet(ClickEvent evt)
    {
        // TODO: Validate input
        
        var key = txtKey.value;
        PlayerPrefs.SetString("API_KEY", key);
        
        // Hide the UI
        gameObject.SetActive(false);
    }
}