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
        buttonNames = new string[] { "Upload","Yes", "No" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickUpload , ClickYes, ClickNo };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        apiCaller = new APICaller();
        alarmfile = document.rootVisualElement.Q<VisualElement>("AlarmFile");
        fileScreen = document.rootVisualElement.Q<VisualElement>("FileScreen");
        fileBar = document.rootVisualElement.Q<ProgressBar>("FileBar");

        fileBar.style.display = DisplayStyle.None;
        alarmfile.style.display = DisplayStyle.None;
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
            tex = new Texture2D(0, 0);
            tex.LoadImage(imageData);
            fileScreen.style.backgroundImage = new StyleBackground(tex);

            alarmfile.style.display = DisplayStyle.Flex;
        
        });

    }

    void ClickYes(ClickEvent evt)
    {
        StartCoroutine(apiCaller.ImageTo(tex, fileBar));
        fileBar.style.display = DisplayStyle.Flex;
        isRunningAPI = true;

        // 확인 창 숨기기
        alarmfile.style.display = DisplayStyle.None;
    }

    void ClickNo(ClickEvent evt)
    {
        // fileScreen에서 이미지를 제거하고 초기화
        fileScreen.style.backgroundImage = null;
        tex = null;

        // 확인 창 숨기기
        alarmfile.style.display = DisplayStyle.None;

        Debug.Log("이미지 삭제됨.");
    }

}
