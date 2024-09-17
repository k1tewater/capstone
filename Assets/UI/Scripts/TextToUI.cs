
using UnityEngine;
using UnityEngine.UIElements;

public class TextToUI : UIManager
{
    
    protected override void Awake()
    {
        buttonNames = new string[] { "Input","alarm_yes","alarm_no" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickInput ,ClickYes,ClickNo};
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;
        VisualElement overlay = document.rootVisualElement.Q<VisualElement>("overlay_bottom");
        overlay.style.display = DisplayStyle.None;

    }


    void ClickInput(ClickEvent evt)
    {
        VisualElement overlay = document.rootVisualElement.Q<VisualElement>("overlay_bottom");
        overlay.style.display = DisplayStyle.Flex;
  
    }
    void ClickYes(ClickEvent evt)
    {
        TextField text = document.rootVisualElement.Q<TextField>("InputTextField");
        Debug.Log(text.value);
        Debug.Log("Input clicked");
    }

    void ClickNo(ClickEvent evt)
    {
        VisualElement overlay = document.rootVisualElement.Q<VisualElement>("overlay_bottom");
        overlay.style.display = DisplayStyle.None;
    }
}
