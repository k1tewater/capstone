using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static NativeCamera;

public class CameraUI : UIManager
{
    APICaller apiCaller;
    protected override void Awake()
    {
        buttonNames = new string[] { "Capture" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCapture };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;

        apiCaller = new APICaller();
    }
    void Update()
    {
        if(!apiCaller.task.IsUnityNull() && isRunningAPI && apiCaller.task.data.running_left_time == -1)
        {
            ObjectManager.SetVisualElementCamera(document.rootVisualElement.Q<VisualElement>("CameraVisual"));
            isRunningAPI = false;
        }
    }
    void ClickCapture(ClickEvent evt)
    {
        Debug.Log("Capture clicked");

        NativeCamera.TakePicture(callback, 2048,true, NativeCamera.PreferredCamera.Front);
    }

    private void callback(string path)
    {
        var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
        
        // 촬영된 이미지의 경로가 유효한지 확인
        if (path != null)
        {
            // 촬영된 이미지를 2D 텍스처로 로드
            Texture2D texture = NativeCamera.LoadImageAtPath(path, 2048 ,false);
            Debug.Log($"height : {texture.height}");
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + path);
                return;
            }

            // 촬영한 이미지를 VisualElement의 백그라운드로 설정
            camVisual.style.backgroundImage = Background.FromTexture2D(texture);

            //API 호출
            APICaller apiCaller = new APICaller();
            StartCoroutine(apiCaller.ImageTo(texture));
            isRunningAPI = true;
        }
    }
}