using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    string[] buttonNames;
    EventCallback<ClickEvent>[] clickEvts;
    void Awake()
    {
        buttonNames = new string[] { "Imageto", "Textto", "List" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickImageTo, ClickTextTo, ClickList };
        Managers.ui.SetClickEvt(gameObject, buttonNames, clickEvts);
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
    }

    void ClickImageTo(ClickEvent evt)
    {
        Debug.Log("Image to clicked");
        Managers.ui.SwitchUI(gameObject, "ImageToUI");
    }

    void ClickTextTo(ClickEvent evt)
    {
        Debug.Log("text to clicked");
    }

    void ClickList(ClickEvent evt)
    {
        Debug.Log("list clicked");
    }
}
