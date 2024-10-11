using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static NativeCamera;

public class CameraUI : UIManager
{
    APICaller apiCaller;
    VisualElement cameraScreen, alarmCamera, alarmSavedName;
    ProgressBar cameraBar;
    Texture2D texture;
    protected override void Awake()
    {
        buttonNames = new string[] { "Capture", "Reupload", "Save", "Yes", "No","SaveConfirm", "Cancel" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCapture, ClickReupload, ClickSave, ClickYes, ClickNo, ClickSaveConfirm, ClickCancel };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        apiCaller = new APICaller();
        alarmCamera = document.rootVisualElement.Q<VisualElement>("AlarmCamera");
        alarmSavedName = document.rootVisualElement.Q<VisualElement>("AlarmSavedName");
        cameraScreen = document.rootVisualElement.Q<VisualElement>("CameraScreen");
        cameraBar = document.rootVisualElement.Q<ProgressBar>("CameraBar");

        cameraBar.style.display = DisplayStyle.None;
        alarmCamera.style.display = DisplayStyle.None;
        alarmSavedName.style.display = DisplayStyle.None;

        GetButton("Reupload").style.display = DisplayStyle.None;
        GetButton("Save").style.display = DisplayStyle.None;
    }
    void Update()
    {
        if (!apiCaller.task.IsUnityNull() && isRunningAPI && apiCaller.task.data.running_left_time == -1)
        {
            Debug.Log("CameraUI upload done");
            objectManager.SetVisualElementCamera(cameraScreen);
            isRunningAPI = false;
        }
    }
    void ClickCapture(ClickEvent evt)
    {
        Debug.Log("Capture clicked");

        NativeCamera.TakePicture(callback, 2048, true, NativeCamera.PreferredCamera.Front);
    }

    void ClickReupload(ClickEvent evt)
    {

    }

    void ClickSave(ClickEvent evt)
    {
        alarmSavedName.style.display = DisplayStyle.Flex;
    }
    private void callback(string path)
    {
        // 촬영된 이미지의 경로가 유효한지 확인
        if (path != null)
        {
            // 촬영된 이미지를 2D 텍스처로 로드
            texture = NativeCamera.LoadImageAtPath(path, 2048, false);
            Debug.Log($"height : {texture.height}");
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + path);
                return;
            }

            // 촬영한 이미지를 VisualElement의 백그라운드로 설정
            cameraScreen.style.backgroundImage = Background.FromTexture2D(texture);
            alarmCamera.style.display = DisplayStyle.Flex;
        } 
    }
    void ClickYes(ClickEvent evt)
    {
        StartCoroutine(apiCaller.ImageTo(texture, cameraBar));
        cameraBar.style.display = DisplayStyle.Flex;
        isRunningAPI = true;

        // 확인 창 숨기기
        alarmCamera.style.display = DisplayStyle.None;

        GetButton("Capture").style.display = DisplayStyle.None;
        GetButton("Reupload").style.display = DisplayStyle.Flex;
        GetButton("Save").style.display = DisplayStyle.Flex;
    }

    void ClickNo(ClickEvent evt)
    {
        // fileScreen에서 이미지를 제거하고 초기화
        cameraScreen.style.backgroundImage = null;
        texture = null;

        // 확인 창 숨기기
        alarmCamera.style.display = DisplayStyle.None;

        Debug.Log("이미지 삭제됨.");
    }
    void ClickSaveConfirm(ClickEvent evt)
    {

    }

    void ClickCancel(ClickEvent evt)
    {
        alarmSavedName.style.display = DisplayStyle.None;
    }
}