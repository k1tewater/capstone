using UnityEngine;
using UnityEngine.UIElements;

public class TextUI : MonoBehaviour
{
    string[] buttonNames;
    EventCallback<ClickEvent>[] clickEvts;
    void Awake()
    {
        buttonNames = new string[] { "Input" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickInput };
        Managers.ui.SetClickEvt(gameObject, buttonNames, clickEvts);
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
    }


    void ClickInput(ClickEvent evt)
    {
        Debug.Log("Input clicked");
    }

}
