using UnityEngine;
using UnityEngine.UIElements;

public class CameraUI : MonoBehaviour
{
    string[] buttonNames;
    EventCallback<ClickEvent>[] clickEvts;
    void Awake()
    {
        buttonNames = new string[] { "Capture" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCapture };
        Managers.ui.SetClickEvt(gameObject, buttonNames, clickEvts);
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
    }


    void ClickCapture(ClickEvent evt)
    {
        Debug.Log("Capture clicked");
    }

}
