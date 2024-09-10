using UnityEngine;
using UnityEngine.UIElements;

public class TextToUI : UIManager
{
    protected override void Awake()
    {
        buttonNames = new string[] { "Input" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickInput };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;
    }


    void ClickInput(ClickEvent evt)
    {
        TextField text = document.rootVisualElement.Q<TextField>("InputTextField");
        Debug.Log(text.value);
        Debug.Log("Input clicked");
    }
}
