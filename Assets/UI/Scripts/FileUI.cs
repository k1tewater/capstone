using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FileUI : UIManager
{

    APICaller apiCaller;
    VisualElement fileScreen , alarmFile, alarmSavedName;
    ProgressBar fileBar;
    Texture2D tex;

    protected override void Awake()
    {
        buttonNames = new string[] { "Upload", "Reupload", "Save", "Yes", "No", "SaveConfirm","Cancel" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickUpload , ClickReupload, ClickSave, ClickYes, ClickNo, ClickSaveConfirm, ClickCancel };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        apiCaller = new APICaller();
        alarmFile = document.rootVisualElement.Q<VisualElement>("AlarmFile");
        alarmSavedName = document.rootVisualElement.Q<VisualElement>("AlarmSavedName");
        fileScreen = document.rootVisualElement.Q<VisualElement>("FileScreen");
        fileBar = document.rootVisualElement.Q<ProgressBar>("FileBar");

        fileBar.style.display = DisplayStyle.None;
        alarmFile.style.display = DisplayStyle.None;
        alarmSavedName.style.display = DisplayStyle.None;

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

            alarmFile.style.display = DisplayStyle.Flex;
        
        });

    }

    void ClickReupload(ClickEvent evt)
    {

    }

    void ClickSave(ClickEvent evt)
    {
        alarmSavedName.style.display = DisplayStyle.Flex;
    }
    void ClickYes(ClickEvent evt)
    {
        StartCoroutine(apiCaller.ImageTo(tex, fileBar));
        fileBar.style.display = DisplayStyle.Flex;
        isRunningAPI = true;

        // Ȯ�� â �����
        alarmFile.style.display = DisplayStyle.None;
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
        alarmFile.style.display = DisplayStyle.None;

        Debug.Log("�̹��� ������.");
    }

    void ClickSaveConfirm(ClickEvent evt)
    {

    }

    void ClickCancel(ClickEvent evt)
    {
        alarmSavedName.style.display = DisplayStyle.None;
    }
}
