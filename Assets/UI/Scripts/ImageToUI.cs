using UnityEngine;
using UnityEngine.UIElements;

public class ImageToUI : MonoBehaviour
{
    string[] buttonNames;
    EventCallback<ClickEvent>[] clickEvts;
    void Awake()
    {
        buttonNames = new string[] { "Camera", "File" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCamera, ClickFile };
        Managers.ui.SetClickEvt(gameObject, buttonNames, clickEvts);
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
    }

    void ClickCamera(ClickEvent evt)
    {
        Debug.Log("Camera clicked");
        Managers.ui.SwitchUI(gameObject, "CameraUI");
    }

    void ClickFile(ClickEvent evt)
    {
        Debug.Log("File clicked");
        Managers.ui.SwitchUI(gameObject, "FileUI");
    }

    void ClickBack(ClickEvent evt)
    {
        Managers.ui.SwitchUI(gameObject, "MainUI");
    }
}
