using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FileUI : UIManager
{

    APICaller apiCaller;
    VisualElement fileScreen , alarmFile, alarmSavedName;
    TextField inputTextField;
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
        inputTextField = document.rootVisualElement.Q<TextField>("inputTextField");
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
            Debug.Log("FileUI Upload Done");
            objectManager.SetVisualElementCamera(fileScreen);
            isRunningAPI = false;
        }
    }

    void ClickUpload(ClickEvent evt)
    {
        Debug.Log("Upload clicked");
        NativeGallery.GetImageFromGallery((file) =>
        {
            byte[] imageData = File.ReadAllBytes(file);
            tex = new Texture2D(0, 0);
            tex.LoadImage(imageData);
            fileScreen.style.backgroundImage = new StyleBackground(tex);

            alarmFile.style.display = DisplayStyle.Flex;
        });
    }

    void ClickReupload(ClickEvent evt)
    {
        if(isRunningAPI)
        {
            StopCoroutine(apiCaller.ImageTo(tex, fileBar));
            fileBar.title = "Task Stopped.";
            fileBar.value = 0f;
            isRunningAPI = false;
        }

        NativeGallery.GetImageFromGallery((file) =>
        {
            byte[] imageData = File.ReadAllBytes(file);
            tex = new Texture2D(0, 0);
            tex.LoadImage(imageData);
            fileScreen.style.backgroundImage = new StyleBackground(tex);

            alarmFile.style.display = DisplayStyle.Flex;
        });
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

        alarmFile.style.display = DisplayStyle.None;
        GetButton("Upload").style.display = DisplayStyle.None;
        GetButton("Reupload").style.display = DisplayStyle.Flex;
        GetButton("Save").style.display = DisplayStyle.Flex;
    }

    void ClickNo(ClickEvent evt)
    {
        fileScreen.style.backgroundImage = null;
        tex = null;

        alarmFile.style.display = DisplayStyle.None;
    }

    void ClickSaveConfirm(ClickEvent evt)
    {
        objectManager.SaveObjectFile(inputTextField.text);
        alarmSavedName.style.display = DisplayStyle.None;

        fileBar.title = $"{inputTextField.text}3D.obj, .mtl, .png are saved";
    }

    void ClickCancel(ClickEvent evt)
    {
        alarmSavedName.style.display = DisplayStyle.None;
    }
}
