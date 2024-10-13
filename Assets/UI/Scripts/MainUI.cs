using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : UIManager
{
    protected override void Awake()
    {
        buttonNames = new string[] { "Imageto", "Textto" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickImageTo, ClickTextTo };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void ClickImageTo(ClickEvent evt)
    {
        Debug.Log("Image to clicked");
        SwitchUI("ImageToUI");
    }

    void ClickTextTo(ClickEvent evt)
    {
        Debug.Log("text to clicked");
        SwitchUI("TextToUI");
    }
}
