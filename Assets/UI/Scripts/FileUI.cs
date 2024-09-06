using UnityEngine;
using UnityEngine.UIElements;

public class FileUI : MonoBehaviour
{
    string[] buttonNames;
    EventCallback<ClickEvent>[] clickEvts;
     void Awake()
    {
        buttonNames = new string[] { "Upload" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickUpload };
        Managers.ui.SetClickEvt(gameObject, buttonNames, clickEvts);
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
    }
    
    void ClickUpload(ClickEvent evt)
    {
        Debug.Log("Upload clicked");
        Managers.ui.LoadImageFromGallery(gameObject, "FileScreen");
    }

}
