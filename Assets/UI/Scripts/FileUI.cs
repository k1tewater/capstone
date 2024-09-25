using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FileUI : UIManager
{
    APICaller apiCaller;
    VisualElement fileScreen;
    ProgressBar fileBar;
    protected override void Awake()
    {
        buttonNames = new string[] { "Upload" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickUpload };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        apiCaller = new APICaller();
        fileScreen = document.rootVisualElement.Q<VisualElement>("FileScreen");
        fileBar = document.rootVisualElement.Q<ProgressBar>("FileBar");

        fileBar.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (!apiCaller.task.IsUnityNull() && isRunningAPI && apiCaller.task.data.running_left_time == -1)
        {
            Debug.Log("FileUI upload done");
            ObjectManager.SetVisualElementCamera(fileScreen);
            isRunningAPI = false;
        }
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

            byte[] imageData = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(imageData);
            fileScreen.style.backgroundImage = new StyleBackground(tex);
            StartCoroutine(apiCaller.ImageTo(tex, fileBar));
            fileBar.style.display = DisplayStyle.Flex;
            isRunningAPI = true;
        });
    }

}
