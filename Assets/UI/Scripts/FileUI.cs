using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class FileUI : UIManager
{
    protected override void Awake()
    {
        buttonNames = new string[] { "Upload" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickUpload };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;
    }
    
    void ClickUpload(ClickEvent evt)
    {
        Debug.Log("Upload clicked");
        NativeGallery.GetImageFromGallery((file) =>
        {
            if (string.IsNullOrEmpty(file))
            {
                Debug.LogWarning("No image selected.");
                return;
            }

            FileInfo selected = new FileInfo(file);
            if (selected.Length > 50000000)
            {
                Debug.LogWarning("Selected image is too large.");
                return;
            }
            var root = document.rootVisualElement;
        VisualElement targetElement = root.Q<VisualElement>("FileScreen");
        if (targetElement == null) 
        {
            Debug.LogWarning($"VisualElement with name {file} not found.");
            return;
        } 

       
        byte[] imageData = File.ReadAllBytes(file);
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(imageData);
        targetElement.style.backgroundImage = new StyleBackground(tex);
        });
    }

}
