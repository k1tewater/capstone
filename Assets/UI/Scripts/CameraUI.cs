using GLTFast.Schema;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using static NativeCamera;

public class CameraUI : UIManager
{
    private int CaptureCounter = 0; // �Կ� ī����
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
        // ī�޶� �Կ� �Լ� ȣ��

    }

    private void callback(string path)
    {
        var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
        
    string SavePath = Application.persistentDataPath;

        NativeCamera.TakePicture((path) =>
        {
            // �Կ��� �̹����� ��ΰ� ��ȿ���� Ȯ��
            if (path != null)
            {
                // �Կ��� �̹����� 2D �ؽ�ó�� �ε�
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 2048);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // �Կ��� �̹����� VisualElement�� ��׶���� ����
                camVisual.style.backgroundImage = Background.FromTexture2D(texture);

                // �Կ��� �̹��� ����
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string galaryPath = SavePath.Substring(0, SavePath.IndexOf("Android")) + "/DCIM/���ϴ� �����̸�/";

                // ������ ������ ����
                if (!Directory.Exists(galaryPath))
                {
                    Directory.CreateDirectory(galaryPath);
                }

                // �̹��� ���Ϸ� ����
                File.WriteAllBytes(galaryPath + "CapturedImage_" + time + CaptureCounter.ToString() + ".png", texture.EncodeToPNG());
                CaptureCounter++;
            }
        }, 2048, true, NativeCamera.PreferredCamera.Front);
    }

    private void callbackplz(string path)
    {
        var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
        
    string SavePath = Application.persistentDataPath;

            // �Կ��� �̹����� ��ΰ� ��ȿ���� Ȯ��
            if (path != null)
            {
                // �Կ��� �̹����� 2D �ؽ�ó�� �ε�
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 2048);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // �Կ��� �̹����� VisualElement�� ��׶���� ����
                camVisual.style.backgroundImage = Background.FromTexture2D(texture);

                // �Կ��� �̹��� ����
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string galaryPath = SavePath.Substring(0, SavePath.IndexOf("Android")) + "/DCIM/���ϴ� �����̸�/";

                // ������ ������ ����
                if (!Directory.Exists(galaryPath))
                {
                    Directory.CreateDirectory(galaryPath);
                }

                // �̹��� ���Ϸ� ����
                File.WriteAllBytes(galaryPath + "CapturedImage_" + time + CaptureCounter.ToString() + ".png", texture.EncodeToPNG());
                CaptureCounter++;
            }
    }
}

            //���忡 ������Ʈ ȭ�鿡 ���¹�
            // Camera camera = GameObject.Find("ObjectView").GetComponent<Camera>();
            // var camVisual = document.rootVisualElement.Q<VisualElement>("CameraVisual");
            // RenderTexture renderTexture = new RenderTexture((int)camVisual.contentRect.width, (int)camVisual.contentRect.height, 16);
            // camera.targetTexture = renderTexture;
            // camVisual.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(camera.targetTexture));
     
