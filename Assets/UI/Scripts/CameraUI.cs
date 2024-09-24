using GLTFast.Schema;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using static NativeCamera;

public class CameraUI : UIManager
{
    private int CaptureCounter = 0; // 촬영 카운터
    protected override void Awake()
    {
        buttonNames = new string[] { "Capture" };
        clickEvts = new EventCallback<ClickEvent>[] { ClickCapture };
        base.Awake();
        document.rootVisualElement.style.display = DisplayStyle.None;
    }

    void ClickCapture(ClickEvent evt)
    {
        Debug.Log("Capture clicked");
        if (NativeCamera.IsCameraBusy())
        {
            return;
        }
        NativeCamera.TakePicture(callbackplz, 2048, true, NativeCamera.PreferredCamera.Front);
        // 카메라 촬영 함수 호출

    }

    private void callback(string path)
    {
        var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
        
    string SavePath = Application.persistentDataPath;

        NativeCamera.TakePicture((path) =>
        {
            // 촬영된 이미지의 경로가 유효한지 확인
            if (path != null)
            {
                // 촬영된 이미지를 2D 텍스처로 로드
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 2048);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // 촬영한 이미지를 VisualElement의 백그라운드로 설정
                camVisual.style.backgroundImage = Background.FromTexture2D(texture);

                // 촬영한 이미지 저장
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string galaryPath = SavePath.Substring(0, SavePath.IndexOf("Android")) + "/DCIM/원하는 폴더이름/";

                // 폴더가 없으면 생성
                if (!Directory.Exists(galaryPath))
                {
                    Directory.CreateDirectory(galaryPath);
                }

                // 이미지 파일로 저장
                File.WriteAllBytes(galaryPath + "CapturedImage_" + time + CaptureCounter.ToString() + ".png", texture.EncodeToPNG());
                CaptureCounter++;
            }
        }, 2048, true, NativeCamera.PreferredCamera.Front);
    }

    private void callbackplz(string path)
    {
        var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
        
    string SavePath = Application.persistentDataPath;

            // 촬영된 이미지의 경로가 유효한지 확인
            if (path != null)
            {
                // 촬영된 이미지를 2D 텍스처로 로드
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 2048);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // 촬영한 이미지를 VisualElement의 백그라운드로 설정
                camVisual.style.backgroundImage = Background.FromTexture2D(texture);

                // 촬영한 이미지 저장
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string galaryPath = SavePath.Substring(0, SavePath.IndexOf("Android")) + "/DCIM/원하는 폴더이름/";

                // 폴더가 없으면 생성
                if (!Directory.Exists(galaryPath))
                {
                    Directory.CreateDirectory(galaryPath);
                }

                // 이미지 파일로 저장
                File.WriteAllBytes(galaryPath + "CapturedImage_" + time + CaptureCounter.ToString() + ".png", texture.EncodeToPNG());
                CaptureCounter++;
            }
    }
}

            //월드에 오브젝트 화면에 띄우는법
            // Camera camera = GameObject.Find("ObjectView").GetComponent<Camera>();
            // var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
            // RenderTexture renderTexture = new RenderTexture((int)camVisual.contentRect.width, (int)camVisual.contentRect.height, 16);
            // camera.targetTexture = renderTexture;
            // camVisual.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(camera.targetTexture));
     
