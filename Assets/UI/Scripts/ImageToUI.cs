using System.IO;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ImageToUI : UIManager
{
    protected override void Awake()
    {
        buttonNames = new string[] { "Camera", "File" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCamera, ClickFile };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;
    }

    void ClickCamera(ClickEvent evt)
    {
        
        Debug.Log("Camera clicked");
        SwitchUI("CameraUI");
    }

    void ClickFile(ClickEvent evt)
    {
        Debug.Log("File clicked");
        SwitchUI("FileUI");
    }
    
}
