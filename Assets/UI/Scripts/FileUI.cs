using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FileUI : UIManager
{

    APICaller apiCaller;
    VisualElement fileScreen , alarmfile;
    ProgressBar fileBar;
    Texture2D tex;

    protected override void Awake()
    {
        buttonNames = new string[] { "Upload", "Reupload", "Save", "Yes", "No" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickUpload , ClickReupload, ClickSave, ClickYes, ClickNo };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        apiCaller = new APICaller();
        alarmfile = document.rootVisualElement.Q<VisualElement>("AlarmFile");
        fileScreen = document.rootVisualElement.Q<VisualElement>("FileScreen");
        fileBar = document.rootVisualElement.Q<ProgressBar>("FileBar");

        fileBar.style.display = DisplayStyle.None;
        alarmfile.style.display = DisplayStyle.None;

        GetButton("Reupload").style.display = DisplayStyle.None;
        GetButton("Save").style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (!apiCaller.task.IsUnityNull() && isRunningAPI && apiCaller.task.data.running_left_time == -1)
        {
            Debug.Log("FileUI upload done");
            objectManager.SetVisualElementCamera(fileScreen);
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
            tex = new Texture2D(0, 0);
            tex.LoadImage(imageData);
            fileScreen.style.backgroundImage = new StyleBackground(tex);

            alarmfile.style.display = DisplayStyle.Flex;
        
        });

    }

    void ClickReupload(ClickEvent evt)
    {

    }

    void ClickSave(ClickEvent evt)
    {
        
    }
    void ClickYes(ClickEvent evt)
    {
        StartCoroutine(apiCaller.ImageTo(tex, fileBar));
        fileBar.style.display = DisplayStyle.Flex;
        isRunningAPI = true;

        // Ȯ�� â �����
        alarmfile.style.display = DisplayStyle.None;
        GetButton("Upload").style.display = DisplayStyle.None;
        GetButton("Reupload").style.display = DisplayStyle.Flex;
        GetButton("Save").style.display = DisplayStyle.Flex;
    }

    void ClickNo(ClickEvent evt)
    {
        // fileScreen���� �̹����� �����ϰ� �ʱ�ȭ
        fileScreen.style.backgroundImage = null;
        tex = null;

        // Ȯ�� â �����
        alarmfile.style.display = DisplayStyle.None;

        Debug.Log("�̹��� ������.");
    }

}
